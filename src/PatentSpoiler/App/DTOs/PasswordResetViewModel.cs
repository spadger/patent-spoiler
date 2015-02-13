using System;
using System.ComponentModel.DataAnnotations;

namespace PatentSpoiler.App.DTOs
{
    public class PasswordResetViewModel
    {
        public Guid Token { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string PasswordConfirmation { get; set; }
    }
}