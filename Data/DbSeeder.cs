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

        var slots = new List<Slot>();


        foreach (var doc in doctors)
        {
            var day = DateTime.Today.AddDays(1);
            bool already = await db.Slots.AnyAsync(s => s.DoctorId == doc.DoctorId && s.StartAt.Date == day.Date);
            if (already) continue;

            slots.AddRange(GenSlots(doc.DoctorId, day, new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(0, 30, 0)));
        }

        static IEnumerable<Slot> GenSlots(int doctorId, DateTime day, TimeSpan start, TimeSpan end, TimeSpan step)
        {
            var t = day.Date + start;
            var limit = day.Date + end;
            while (t < limit)
            {
                yield return new Slot
                {
                    DoctorId = doctorId,
                    StartAt = t,
                    EndAt = t + step
                };
                t += step;
            }
        }

        var tomorrow = DateTime.Today.AddDays(1);
        var plus2 = DateTime.Today.AddDays(2);


        foreach (var doc in doctors)
        {
            
            for(int i = 1; i <= 30; i++)
            {
                var day = DateTime.Today.AddDays(i);
                slots.AddRange(GenSlots(doc.DoctorId,day,
                    new TimeSpan(9,0,0),
                    new TimeSpan(12,0,0),
                    new TimeSpan(9,30,0)));
            }
        }

     
        var existingKeys = await db.Slots
            .Select(s => new { s.DoctorId, s.StartAt })
            .ToListAsync();

        var seen = new HashSet<(int, DateTime)>(
            existingKeys.Select(k => (k.DoctorId, k.StartAt))
        );

     
        var generated = slots;

   
        foreach (var s in generated)
        {
            var key = (s.DoctorId, s.StartAt);
            if (seen.Add(key))           
                db.Slots.Add(s);
        }

        await db.SaveChangesAsync();

        static DateTime Next(DayOfWeek target)
        {
            var d = DateTime.Today;
            while (d.DayOfWeek != target) d = d.AddDays(1);
            return d;
        }


    }
}