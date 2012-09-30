using System.Web;
using Penedating.Service.Model;

namespace Penedating.Web.Security
{
    public class UserState
    {
        public string Email { get; set; }
        public UserAccessToken AccessToken { get; set; }

        public static UserState Current
        {
            get
            {
                var userState = HttpContext.Current.Session[MvcApplication.UserStateCookieName] as UserState;

                return userState;
            }
        }
    }
}