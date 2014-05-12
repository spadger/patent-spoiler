using Microsoft.AspNet.Identity;

namespace PatentSpoiler.App.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}