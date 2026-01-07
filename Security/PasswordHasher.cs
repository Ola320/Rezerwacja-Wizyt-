using System.Security.Cryptography;

namespace WebApplication1.Security
{
    public static class PasswordHasher
    {
        // Tworzy hash+salt do zapisu w DB
        public static (string hashB64, string saltB64, int iterations) Hash(string password, int iterations = 100_000)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                32
            );

            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt), iterations);
        }

        // Sprawdza hasło podczas logowania
        public static bool Verify(string password, string saltB64, string expectedHashB64, int iterations)
        {
            byte[] salt = Convert.FromBase64String(saltB64);
            byte[] expected = Convert.FromBase64String(expectedHashB64);

            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                32
            );

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
    }
}