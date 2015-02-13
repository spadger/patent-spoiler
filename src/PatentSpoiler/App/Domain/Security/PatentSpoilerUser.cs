using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace PatentSpoiler.App.Domain.Security
{
    public class PatentSpoilerUser : IUser
    {
        public PatentSpoilerUser()
        {
            Roles = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        string IUser<string>.UserName { get { return Id; } set { Id = value; } }
        
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool Phone { get; set; }
        public DateTime? LockedOutUntil { get; set; }
        public string Passwordhash { get; set; }
        public Guid? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public DateTime? MemberSince { get; set; }

        public HashSet<UserRole> Roles { get; set; }

        public Guid NewPasswordResetToken()
        {
            PasswordResetToken = Guid.NewGuid();
            PasswordResetTokenExpiry = DateTime.Now.AddDays(5);

            return PasswordResetToken.Value;
        }

        public void UpdatePassword(string passwordHash)
        {
            Passwordhash = passwordHash;
            PasswordResetToken = null;
            PasswordResetTokenExpiry = null;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<PatentSpoilerUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}