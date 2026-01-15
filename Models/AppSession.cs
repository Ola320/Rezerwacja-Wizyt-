using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AppSession
    {
        public int Id { get; set; }

       
        [Required]
        public string Token { get; set; } = "";

       
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;

       
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; set; }

       
        public DateTime? RevokedAtUtc { get; set; }
    }
}
