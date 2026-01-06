using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DoctorController : Controller 
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? specializationId)
        {
            var specs = _context.Specializations.OrderBy(s => s.SpecializationName).ToList();
            ViewBag.Specializations = specs;

            var q = _context.Doctors
                .Include(d => d.DoctorSpecializations).ThenInclude(ds => ds.Specialization)
                .AsQueryable();

            if (specializationId.HasValue)
                q = q.Where(d => d.DoctorSpecializations.Any(ds => ds.SpecializationId == specializationId));

            return View(q.ToList());
        }

        public IActionResult Slots(int id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d=> d.DoctorId == id);
            if (doctor == null)
                return NotFound();

            var slots = _context.Slots
                .Include(s => s.Booking)
                .Where(s => s.DoctorId == id && s.Booking == null && s.StartAt > DateTime.Now)
                .OrderBy(s => s.StartAt)
                .ToList();

            ViewBag.Doctor = doctor; 

            return View(slots);
        }
    }
}

