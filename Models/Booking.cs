namespace WebApplication1.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int PatientId {  get; set; }

        public Patient Patient { get; set; } = null!;   
        public int SlotId { get; set; }
        public Slot Slot { get; set; } = null!;
        public DateTime? BookedAt { get; set; } = DateTime.UtcNow;



    }
}
