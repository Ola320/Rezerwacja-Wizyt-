using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

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
            var slot = _context.Slots
                .Include(s => s.Booking)
                .FirstOrDefault(s => s.SlotId == slotId);

            if (slot == null || slot.Booking != null)
                return NotFound("Ten termin jest już zajęty.");

           
            _context.Patients.Add(patient);
            _context.SaveChanges();

      
            var booking = new Booking
            {
                PatientId = patient.PatientId,
                SlotId = slotId,
                BookedAt = DateTime.Now
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
    }
}