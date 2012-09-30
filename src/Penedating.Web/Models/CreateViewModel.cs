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
        public string Username { get; set; }

        [Required(ErrorMessage = "Street not filled")]
        public string StreetAddress { get; set; }

        [Range(999, 9999, ErrorMessage = "Your Zipcode must be between 999-9999")]
        public int ZipCode { get; set; }

        [Required(ErrorMessage = "City not filled")]
        public string City { get; set; }
    }
}