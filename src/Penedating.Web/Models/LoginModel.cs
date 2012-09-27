﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Penedating.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Password Length not valud")]
        public string Password { get; set; }
    }
}