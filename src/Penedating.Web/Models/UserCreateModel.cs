using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Penedating.Web.Models
{
    public class UserCreateModel : LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Range(999, 9999)]
        public int ZipCode { get; set; }

        [Required]
        public string City { get; set; }
    }
}