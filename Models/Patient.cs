using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Surname { get; set; } = string.Empty ;

        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string? Phone { get; set; }
        public string? Address { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
