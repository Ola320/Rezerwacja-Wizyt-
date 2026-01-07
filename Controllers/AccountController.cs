using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Security;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        private const string CookieName = "session_token";
        private static readonly TimeSpan SessionLifetime = TimeSpan.FromDays(7);

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // -------------------------
        // REGISTER
        // -------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Walidacja z DataAnnotations
            if (!ModelState.IsValid)
                return View(model);

            // Normalizacja
            model.Email = (model.Email ?? "").Trim().ToLowerInvariant();
            model.Pesel = (model.Pesel ?? "").Trim();

            // Proste walidacje dodatkowe (opcjonalnie)
            if (model.Pesel.Length != 11 || !model.Pesel.All(char.IsDigit))
            {
                ModelState.AddModelError(nameof(model.Pesel), "PESEL musi mieć 11 cyfr.");
                return View(model);
            }

            // Unikalność Email
            bool emailExists = await _db.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Konto z takim adresem email już istnieje.");
                return View(model);
            }

            // Unikalność PESEL
            bool peselExists = await _db.Users.AnyAsync(u => u.Pesel == model.Pesel);
            if (peselExists)
            {
                ModelState.AddModelError(nameof(model.Pesel), "Konto z takim PESEL już istnieje.");
                return View(model);
            }

            // Hash hasła (PBKDF2)
            var (hash, salt, iterations) = PasswordHasher.Hash(model.Password);

            var user = new AppUser
            {
                Pesel = model.Pesel,
                Name = model.Name.Trim(),
                Surname = model.Surname.Trim(),
                Email = model.Email,
                Address = model.Address?.Trim() ?? "",
                PasswordHash = hash,
                PasswordSalt = salt,
                Iterations = iterations,
                CreatedAtUtc = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Auto-login po rejestracji (możesz zmienić na redirect do Login)
            await CreateSessionAndSetCookie(user.Id);

            return RedirectToAction("Index", "Home");
        }

        // -------------------------
        // LOGIN
        // -------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = (model.Email ?? "").Trim().ToLowerInvariant();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
                return View(model);
            }

            bool ok = PasswordHasher.Verify(
                model.Password,
                user.PasswordSalt,
                user.PasswordHash,
                user.Iterations
            );

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
                return View(model);
            }

            await CreateSessionAndSetCookie(user.Id);

            return RedirectToAction("Index", "Home");
        }

        // -------------------------
        // LOGOUT
        // -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue(CookieName, out var token) &&
                !string.IsNullOrWhiteSpace(token))
            {
                var session = await _db.Sessions
                    .FirstOrDefaultAsync(s => s.Token == token && s.RevokedAtUtc == null);

                if (session != null)
                {
                    session.RevokedAtUtc = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }

            Response.Cookies.Delete(CookieName);

            return RedirectToAction("Login");
        }

        // -------------------------
        // Helper: create session + cookie
        // -------------------------
        private async Task CreateSessionAndSetCookie(int userId)
        {
            // token: 32 losowe bajty -> base64url (cookie-safe)
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

            var now = DateTime.UtcNow;

            var session = new AppSession
            {
                UserId = userId,
                Token = token,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(SessionLifetime),
                RevokedAtUtc = null
            };

            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            Response.Cookies.Append(CookieName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,          // w produkcji: true (HTTPS)
                SameSite = SameSiteMode.Lax,
                Expires = session.ExpiresAtUtc
            });
        }
    }
}