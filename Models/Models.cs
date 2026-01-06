namespace WebApplication1.Models
{
    public class Models
    {
         public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

   
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
        public ICollection<Slot> Slots { get; set; }
    }
}
