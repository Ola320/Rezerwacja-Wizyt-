namespace WebApplication1.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Address { get; set; }
        
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set;  }
        public ICollection<Slot> Slots { get; set; }
    }

}
