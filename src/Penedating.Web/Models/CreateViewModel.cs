using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Penedating.Web.Models
{
    public class CreateViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Username not filled")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 16 characters in length")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Street not filled")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "STreet Address must be between 5 and 30 characters in length")]
        public string StreetAddress { get; set; }

        [Range(999, 9999, ErrorMessage = "Your Zipcode must be between 999-9999")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "City not filled")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "STreet Address must be between 3 and 20 characters in length")]
        public string City { get; set; }
    }
}