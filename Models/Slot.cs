namespace WebApplication1.Models
{
    public class Slot
    {
        public int SlotId { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public DateTime StartAt { get; set;}
        public DateTime EndAt { get; set;}

        public Booking? Booking { get; set;}

    }
}
