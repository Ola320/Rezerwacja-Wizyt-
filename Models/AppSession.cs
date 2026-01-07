using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AppSession
    {
        public int Id { get; set; }

        // losowy, trudny do zgadnięcia token
        [Required]
        public string Token { get; set; } = "";

        // do kogo należy sesja
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;

        // czas życia sesji
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; set; }

        // unieważnienie (logout)
        public DateTime? RevokedAtUtc { get; set; }
    }
}
