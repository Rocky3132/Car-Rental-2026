using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace assigment.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
     
        public string HashedPassword { get; set; }

        // Task 1.2: SHA-256 is used for secure one-way password hashing
        // Task 2.1: AES is used for symmetric encryption of sensitive data
        // Task 1.2: SHA-256 Hashing [cite: 47]
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

       // Task 2.1: AES Encryption for sensitive data [cite: 52]
        public static string Encrypt(string text, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(key, iv);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs)) sw.Write(text);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}