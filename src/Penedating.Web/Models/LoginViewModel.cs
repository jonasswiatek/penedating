using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Penedating.Web.Models.DataAnnotations;

namespace Penedating.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailValidation(ErrorMessage = "Email not valid")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 30 characters in length")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not valid")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "8-32 characters")]
        public string Password { get; set; }
    }
}