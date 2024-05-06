using reservas.api.Services.IService;
using System.Security.Cryptography;
using System.Text;

namespace reservas.api.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public string GeneratePasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] salt = GenerateSalt();

                byte[] passwordWithSalt = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));

                byte[] passwordHashBytes = sha256.ComputeHash(passwordWithSalt);

                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(passwordHashBytes);
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            string[] parts = passwordHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }
            byte[] salt = Convert.FromBase64String(parts[0]);

            byte[] passwordWithSalt = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));

            using (var sha256 = SHA256.Create())
            {
                byte[] passwordHashBytes = sha256.ComputeHash(passwordWithSalt);

                var saida = Convert.ToBase64String(passwordHashBytes);
                return  saida == parts[1];
            }
        }

        private byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var salt = new byte[32];
                randomNumberGenerator.GetBytes(salt);
                return salt;
            }
        }
    }
}
