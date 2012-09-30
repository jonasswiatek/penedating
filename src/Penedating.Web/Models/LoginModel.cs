using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Penedating.Web.Models.DataAnnotations;

namespace Penedating.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailValidation(ErrorMessage = "Email not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not valid")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "8-32 characters")]
        public string Password { get; set; }
    }
}