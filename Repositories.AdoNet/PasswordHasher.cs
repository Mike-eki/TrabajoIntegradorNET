using System;
using System.Security.Cryptography;
using System.Text;

namespace ADO.NET
{
    public static class PasswordHasher
    {
        // Sal estática legacy (para migración; remover después de migrar todos los usuarios)
        private static readonly byte[] LegacyStaticSalt = Encoding.UTF8.GetBytes("MyAppStaticSalt");

        /// <summary>
        /// Computes a SHA-256 hash of the password using a provided or generated salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">Optional existing salt; if null, a new 32-byte salt is generated.</param>
        /// <returns>A tuple containing the Base64-encoded hash and the Base64-encoded salt.</returns>
        public static (string Hash, string Salt) ComputeHash(string password, string? salt = null)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            // Generate a new salt if none provided
            byte[] saltBytes = !string.IsNullOrEmpty(salt)
                ? Convert.FromBase64String(salt)
                : RandomNumberGenerator.GetBytes(32); // 32 bytes for strong salt

            // Combine password and salt
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var salted = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, salted, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, salted, saltBytes.Length, passwordBytes.Length);

            // Compute SHA-256 hash
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(salted);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        /// <summary>
        /// Verifies a password against a stored hash using the stored salt.
        /// </summary>
        public static bool Verify(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
                return false;

            var (computedHash, _) = ComputeHash(password, storedSalt);
            return computedHash == storedHash;
        }

        /// <summary>
        /// Legacy verification method using static salt (for migration only).
        /// </summary>
        public static bool VerifyLegacy(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            // Combine password and legacy static salt
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var salted = new byte[LegacyStaticSalt.Length + passwordBytes.Length];
            Buffer.BlockCopy(LegacyStaticSalt, 0, salted, 0, LegacyStaticSalt.Length);
            Buffer.BlockCopy(passwordBytes, 0, salted, LegacyStaticSalt.Length, passwordBytes.Length);

            // Compute SHA-256 hash
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(salted);
            var computedHash = Convert.ToBase64String(hashBytes);

            return computedHash == storedHash;
        }
    }
}