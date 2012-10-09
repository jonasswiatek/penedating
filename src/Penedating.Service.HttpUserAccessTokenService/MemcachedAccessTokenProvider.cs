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
            var tokenHash = GetHashFromCookie();
            object storedObject;
            if (_memcachedClient.TryGet(tokenHash, out storedObject))
            {
                var accessToken = storedObject as UserAccessToken;
                if(accessToken == null)
                {
                    _logger.Warn("Memcached contained something that wasn't an access token: " + tokenHash + ", it was a: " + storedObject.GetType().FullName);
                    throw new UnknownUserTokenHashException();
                }

                _logger.Info("Successfully retrived access token for user: " + accessToken.Email);
                return accessToken;
            }

            _logger.Warn("A user attempted to load an invalid access token: " + tokenHash);
            throw new UnknownUserTokenHashException();
        }

        public bool TryGetAccessToken(out UserAccessToken accessToken)
        {
            try
            {
                object token;
                var result = _memcachedClient.TryGet(GetHashFromCookie(), out token);

                accessToken = token as UserAccessToken;
                return accessToken != null;
            }
            catch(NoUserAccessTokenFoundException)
            {
                accessToken = null;
                return false;
            }
        }

        public void DestroyAccessToken()
        {
            var hash = GetHashFromCookie();
            _logger.Info("Destroying session: " + hash);

            _memcachedClient.Remove(hash);
            var cookie = new HttpCookie(_cookieName, "expired")
                             {
                                 Secure = _useSecureCookie,     /* Ensures that this cookie is only used on SSL connections - this prevents Man-in-the-middle attacks */
                                 HttpOnly = true,               /* Ensures that the cookie cannot be read from JavaScript - this prevents XSS attacks */
                                 Expires = DateTime.Now.AddYears(-1)
                             };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private string GetHashFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[_cookieName];
            if (cookie == null)
            {
                _logger.Debug("Tried to load access token from cookie, but it did not exist");
                throw new NoUserAccessTokenFoundException();
            }

            return cookie.Value;
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