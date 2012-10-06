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

        public MemcachedAccessTokenProvider(IMemcachedClient memcachedClient, string cookieName)
        {
            _memcachedClient = memcachedClient;
            _cookieName = cookieName;
        }

        public void SetUserAccessToken(UserAccessToken accessToken)
        {
            var hash = GenerateSaltedHash(accessToken.Email + ":" + accessToken.Ticket, GenerateSalt());
            var hashString = Convert.ToBase64String(hash);

            if (!_memcachedClient.Store(StoreMode.Set, hashString, accessToken))
            {
                throw new UserTokenPersistenceFailedExpcetion();
            }

            var cookie = new HttpCookie(_cookieName, hashString);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public UserAccessToken GetAccessToken()
        {
            object storedObject;
            if (_memcachedClient.TryGet(GetHashFromCookie(), out storedObject))
            {
                var accessToken = storedObject as UserAccessToken;
                if(accessToken == null)
                {
                    throw new UnknownUserTokenHashException();
                }

                return accessToken;
            }

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
            _memcachedClient.Remove(hash);
            var cookie = new HttpCookie(_cookieName, "expired");
            cookie.Expires = DateTime.Now.AddYears(-1);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private string GetHashFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[_cookieName];
            if (cookie == null)
            {
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