using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class EditBookingViewModel
    {
        public int BookingId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int SlotId { get; set; }

        public List<SelectListItem> Doctors { get; set; } = new();
        public List<SelectListItem> Slots { get; set; } = new();

       
        public string? CurrentDoctorName { get; set; }
        public DateTime? CurrentStartAt { get; set; }
        public DateTime? CurrentEndAt { get; set; }
    }
}