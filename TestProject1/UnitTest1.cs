using Xunit;
using assigment.Models;
using assigment.Servicess;

namespace UnitProject
{
    public class SecurityTests
    {
        // Task 4.1: Verify User Authentication and Hashing
        [Fact]
        public void Test_PasswordHashing()
        {
            // Arrange
            string pass = "Wipro123";

            // Act: Hash the password twice to check for consistency
            string hash1 = User.HashPassword(pass);
            string hash2 = User.HashPassword(pass);

            // Assert
            // Verifies that the same input produces the same hash
            Assert.Equal(hash1, hash2);
            // Verifies that passwords are not stored in plain text
            Assert.NotEqual(pass, hash1);
        }

        // Task 4.2: Verify Data Encryption
        [Fact]
        public void Test_Encryption()
        {
            // Arrange
            byte[] key = new byte[32]; // 256-bit AES key
            byte[] iv = new byte[16];
            string original = "SecretData";

            // Act: Encrypt the string
            string encrypted = User.Encrypt(original, key, iv);

            // Assert
            // Verifies that sensitive data is actually transformed/secured
            Assert.NotEqual(original, encrypted);
        }

        // Task 4.3: Verify Error Handling and Reliable Login
        [Fact]
        public void Test_InvalidLogin_ReturnsFalse()
        {
            // Arrange
            var service = new UserService();

            // Act: Try to login with a user that doesn't exist
            bool result = service.Login("NonExistentUser", "SomePass");

            // Assert
            // Ensures the system handles incorrect data gracefully without crashing
            Assert.False(result);
        }
    }
}