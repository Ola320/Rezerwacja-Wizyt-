using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        public IActionResult Create(int slotId)
        {
            var slot = _context.Slots
                .Include(s => s.Doctor)
                .FirstOrDefault(s => s.SlotId == slotId);

            if (slot == null || slot.Booking != null)
                return NotFound("Ten termin jest już zajęty lub nie istnieje.");

            ViewBag.Slot = slot;
            return View(new Patient());
        }

  
        [HttpPost]
        public IActionResult Create(int slotId, Patient patient)
        {
            
            var user = HttpContext.Items["User"] as AppUser;
            if (user == null)
                return RedirectToAction("Login", "Account");

            var slot = _context.Slots
                .Include(s => s.Booking)
                .FirstOrDefault(s => s.SlotId == slotId);

            if (slot == null || slot.Booking != null)
                return NotFound("Ten termin jest już zajęty.");

           
            _context.Patients.Add(patient);
            _context.SaveChanges();

            
            var booking = new Booking
            {
                UserId = user.Id,             
                PatientId = patient.PatientId,
                SlotId = slotId,
                BookedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Confirm", new { bookingId = booking.BookingId });
        }


        public IActionResult Confirm(int bookingId)
        {
            var booking = _context.Bookings
                .Include(b => b.Patient)
                .Include(b => b.Slot)
                    .ThenInclude(s => s.Doctor)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpGet]
        public IActionResult MyVisits()
        {
            var user = HttpContext.Items["User"] as AppUser;
            if (user == null)
                return RedirectToAction("Login", "Account");

            var visits = _context.Bookings
                .Where(b => b.UserId == user.Id)
                .Include(b => b.Slot)
                    .ThenInclude(s => s.Doctor)
                .Include(b => b.Patient)
                .OrderByDescending(b => b.BookedAt)
                .ToList();

            return View(visits);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // [RequireLogin]
        public IActionResult Cancel(int bookingId)
        {
            var user = HttpContext.Items["User"] as AppUser;
            if (user == null)
                return RedirectToAction("Login", "Account");

            var booking = _context.Bookings
                .Include(b => b.Slot)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
                return NotFound();

            // zabezpieczenie: tylko właściciel może odwołać
            if (booking.UserId != user.Id)
                return Forbid();

            if (!booking.IsCanceled)
            {
                booking.IsCanceled = true;
                booking.CanceledAtUtc = DateTime.UtcNow;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(MyVisits));
        }

        [HttpGet]
        public IActionResult Edit(int bookingId, int? doctorId = null)
        {
            var user = HttpContext.Items["User"] as AppUser;
            if (user == null) return RedirectToAction("Login", "Account");

            var booking = _context.Bookings
                .Include(b => b.Slot).ThenInclude(s => s.Doctor)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) return NotFound();
            if (booking.UserId != user.Id) return Forbid();
            if (booking.IsCanceled) return BadRequest("Nie można edytować odwołanej wizyty.");

            // lekarz wybrany w querystring albo aktualny
            int selectedDoctorId = doctorId ?? booking.Slot.DoctorId;

            var vm = new EditBookingViewModel
            {
                BookingId = booking.BookingId,
                DoctorId = selectedDoctorId,
                SlotId = booking.SlotId,

                CurrentDoctorName = $"{booking.Slot.Doctor.Name} {booking.Slot.Doctor.Surname}",
                CurrentStartAt = booking.Slot.StartAt,
                CurrentEndAt = booking.Slot.EndAt
            };

            var currentDoctorSpecializationIds = _context.DoctorSpecializations
                .Where(ds => ds.DoctorId == booking.Slot.DoctorId)
                .Select(ds => ds.SpecializationId)
                .ToList();

            // lekarze, którzy mają przynajmniej jedną z tych specjalizacji
            var allowedDoctors = _context.Doctors
                .Where(d =>
                    d.DoctorSpecializations.Any(ds =>
                        currentDoctorSpecializationIds.Contains(ds.SpecializationId)))
                .OrderBy(d => d.Surname)
                .ThenBy(d => d.Name)
                .ToList();

            vm.Doctors = allowedDoctors.Select(d => new SelectListItem
            {
                Value = d.DoctorId.ToString(),
                Text = $"{d.Name} {d.Surname}",
                Selected = d.DoctorId == selectedDoctorId
            }).ToList();

            // sloty dla wybranego lekarza:
            // - wolne (Booking == null)
            // - plus aktualny slot tej rezerwacji (żeby dało się zostawić bez zmiany)
            var slots = _context.Slots
                .Include(s => s.Booking)
                .Where(s => s.DoctorId == selectedDoctorId)
                .Where(s => s.Booking == null || s.SlotId == booking.SlotId)
                .OrderBy(s => s.StartAt)
                .ToList();

            vm.Slots = slots.Select(s => new SelectListItem
            {
                Value = s.SlotId.ToString(),
                Text = $"{s.StartAt.ToLocalTime():yyyy-MM-dd HH:mm} - {s.EndAt.ToLocalTime():HH:mm}" +
                       (s.SlotId == booking.SlotId ? " (aktualny)" : ""),
                Selected = s.SlotId == booking.SlotId
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditBookingViewModel model)
        {
            var user = HttpContext.Items["User"] as AppUser;
            if (user == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Edit), new { bookingId = model.BookingId, doctorId = model.DoctorId });

            var booking = _context.Bookings
                .Include(b => b.Slot)
                .FirstOrDefault(b => b.BookingId == model.BookingId);

            if (booking == null) return NotFound();
            if (booking.UserId != user.Id) return Forbid();
            if (booking.IsCanceled) return BadRequest("Nie można edytować odwołanej wizyty.");

            var newSlot = _context.Slots
                .Include(s => s.Booking)
                .FirstOrDefault(s => s.SlotId == model.SlotId);

            if (newSlot == null) return NotFound("Wybrany termin nie istnieje.");

            // slot musi należeć do wybranego lekarza
            if (newSlot.DoctorId != model.DoctorId)
                return BadRequest("Wybrany termin nie pasuje do lekarza.");

            // slot musi być wolny (albo to aktualny slot tej rezerwacji)
            if (newSlot.Booking != null && newSlot.SlotId != booking.SlotId)
                return BadRequest("Ten termin jest już zajęty.");

            // ✅ zmiana terminu
            booking.SlotId = newSlot.SlotId;
            _context.SaveChanges();

            return RedirectToAction(nameof(MyVisits));
        }
    }
}