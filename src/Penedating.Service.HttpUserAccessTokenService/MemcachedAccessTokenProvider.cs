using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Penedating.Service.HttpUserAccessTokenService.Exceptions;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;

namespace Penedating.Service.HttpUserAccessTokenService
{
    public class MemcachedAccessTokenProvider : IUserAccessTokenProvider
    {
        private readonly IMemcachedClient _memcachedClient;
        private readonly string _cookieName;
        private readonly bool _useSecureCookie;
        private readonly log4net.ILog _logger;

        private UserAccessToken CurrentAccessToken
        {
            get
            {
                var current = HttpContext.Current.Items[typeof(UserAccessToken)] as UserAccessToken;
                return current;
            }
            set
            {
                HttpContext.Current.Items[typeof(UserAccessToken)] = value;
            }
        }

        public MemcachedAccessTokenProvider(IMemcachedClient memcachedClient, string cookieName, bool useSecureCookie)
        {
            _memcachedClient = memcachedClient;
            _cookieName = cookieName;
            _useSecureCookie = useSecureCookie;
            _logger = log4net.LogManager.GetLogger(this.GetType());
        }

        public void SetUserAccessToken(UserAccessToken accessToken)
        {
            var hash = GenerateSaltedHash(accessToken.Email + ":" + accessToken.Ticket, GenerateSalt());
            var hashString = Convert.ToBase64String(hash);

            if (!_memcachedClient.Store(StoreMode.Set, hashString, accessToken))
            {
                _logger.Error("Failed to set access token for: " + accessToken.Email);
                throw new UserTokenPersistenceFailedExpcetion();
            }

            CurrentAccessToken = accessToken;

            var cookie = new HttpCookie(_cookieName, hashString)
                             {
                                 Secure = _useSecureCookie,     /* Ensures that this cookie is only used on SSL connections - this prevents Man-in-the-middle attacks */
                                 HttpOnly = true,               /* Ensures that the cookie cannot be read from JavaScript - this prevents XSS attacks */
                             };
            HttpContext.Current.Response.Cookies.Add(cookie);

            _logger.Info("Set access token for: " + accessToken.Email);
        }

        public UserAccessToken GetAccessToken()
        {
            UserAccessToken accessToken;
            if(!TryGetAccessToken(out accessToken))
            {
                throw new UnknownUserTokenHashException();
            }

            return accessToken;
        }

        public bool TryGetAccessToken(out UserAccessToken accessToken)
        {
            if(CurrentAccessToken != null)
            {
                accessToken = CurrentAccessToken;
                return true;
            }

            string hash;
            if(TryGetHashFromCookie(out hash))
            {
                accessToken = _memcachedClient.Get<UserAccessToken>(hash);
                CurrentAccessToken = accessToken;
                return accessToken != null;
            }

            accessToken = null;
            return false;
        }

        public void DestroyAccessToken()
        {
            string hash;
            if(TryGetHashFromCookie(out hash))
            {
                _logger.Info("Destroying session: " + hash);

                _memcachedClient.Remove(hash);
                HttpContext.Current.Items.Remove(typeof(UserAccessToken));

                var cookie = new HttpCookie(_cookieName, "expired")
                {
                    Secure = _useSecureCookie,     /* Ensures that this cookie is only used on SSL connections - this prevents Man-in-the-middle attacks */
                    HttpOnly = true,               /* Ensures that the cookie cannot be read from JavaScript - this prevents XSS attacks */
                    Expires = DateTime.Now.AddYears(-1)
                };

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        private bool TryGetHashFromCookie(out string hashString)
        {
            var cookie = HttpContext.Current.Request.Cookies[_cookieName];
            if (cookie == null)
            {
                hashString = null;
                return false;
            }

            hashString = cookie.Value;
            return true;
        }

        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            var password = Encoding.UTF8.GetBytes(plainText);

            var salted = password.Concat(salt).ToArray();
            return algorithm.ComputeHash(salted);
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[25];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            return salt;
        }
    }
}