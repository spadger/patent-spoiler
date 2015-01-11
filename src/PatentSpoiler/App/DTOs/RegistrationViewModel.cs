using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatentSpoiler.App.Domain.Security;

namespace PatentSpoiler.App.DTOs
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public PatentSpoilerUser ToUser()
        {
            return new PatentSpoilerUser
            {
                Id = Username,
                Email = Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(Password),
                MemberSince = DateTime.Now,
                Roles = new HashSet<UserRole> { UserRole.UnverifiedMember }
            };
        }
    }
}