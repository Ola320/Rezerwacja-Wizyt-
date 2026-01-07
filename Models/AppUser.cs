using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        public string Pesel { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Surname { get; set; } = "";

        public string Address { get; set; } = "";

        // 🔐 bezpieczeństwo – TYLKO HASH
        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string PasswordSalt { get; set; } = "";

        public int Iterations { get; set; } = 100_000;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}