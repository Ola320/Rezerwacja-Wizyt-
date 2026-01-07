using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        var doctors = await db.Doctors.OrderBy(d => d.DoctorId).ToListAsync();
        if (doctors.Count == 0) return;

        // 🔹 Ustawienia generowania slotów
        int daysToGenerate = 30;
        TimeSpan step = TimeSpan.FromMinutes(30);

        // Dwa bloki godzin (możesz zmienić)
        var morningStart = new TimeSpan(9, 0, 0);
        var morningEnd = new TimeSpan(12, 0, 0);
        var afternoonStart = new TimeSpan(13, 0, 0);
        var afternoonEnd = new TimeSpan(18, 30, 0);

        // Pobierz istniejące klucze, żeby nie dublować
        var existingKeys = await db.Slots
            .Select(s => new { s.DoctorId, s.StartAt })
            .ToListAsync();

        var seen = new HashSet<(int DoctorId, DateTime StartAt)>(
            existingKeys.Select(k => (k.DoctorId, k.StartAt))
        );

        var newSlots = new List<Slot>();

        foreach (var doc in doctors)
        {
            for (int i = 1; i <= daysToGenerate; i++)
            {
                var day = DateTime.Today.AddDays(i).Date;

                // rano
                AddSlotsForRange(newSlots, doc.DoctorId, day, morningStart, morningEnd, step);

                // popołudnie
                AddSlotsForRange(newSlots, doc.DoctorId, day, afternoonStart, afternoonEnd, step);
            }
        }

        foreach (var s in newSlots)
        {
            var key = (s.DoctorId, s.StartAt);
            if (seen.Add(key))
                db.Slots.Add(s);
        }

        await db.SaveChangesAsync();
    }

    private static void AddSlotsForRange(List<Slot> list, int doctorId, DateTime day, TimeSpan start, TimeSpan end, TimeSpan step)
    {
        var t = day.Date + start;
        var limit = day.Date + end;

        // tworzy sloty: [t, t+step] aż do limitu
        while (t + step <= limit)
        {
            list.Add(new Slot
            {
                DoctorId = doctorId,
                StartAt = t,
                EndAt = t + step
            });

            t += step;
        }
    }
}