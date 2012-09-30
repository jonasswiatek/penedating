using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Penedating.Service.Model;

namespace Penedating.Web.Models
{
    public class UserState
    {
        public string Email { get; set; }
        public UserAccessToken AccessToken { get; set; }
    }
}