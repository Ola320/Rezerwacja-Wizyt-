namespace WebApplication1.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;


        public int PatientId {  get; set; }
        public Patient Patient { get; set; } = null!;   
        public int SlotId { get; set; }
        public Slot Slot { get; set; } = null!;
        public DateTime? BookedAt { get; set; } = DateTime.UtcNow;

        public bool IsCanceled { get; set; } = false;

        public DateTime? CanceledAtUtc { get; set; }

    }
}
