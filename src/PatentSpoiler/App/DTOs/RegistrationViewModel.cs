using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatentSpoiler.App.Domain.Security;

namespace PatentSpoiler.App.DTOs
{
    public class RegistrationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirmation { get; set; }

        public bool RememberMe { get; set; }

        public PatentSpoilerUser ToUser()
        {
            return new PatentSpoilerUser
            {
                Id = Username,
                Email = Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(Password),
                MemberSince = DateTime.Now
            };
        }
    }
}