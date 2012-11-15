using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Penedating.Data.MongoDB;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Service.HttpUserAccessTokenService;
using Penedating.Service.HttpUserAccessTokenService.Transcoders;
using Penedating.Service.Model.Contract;
using Penedating.Service.MongoService;
using StructureMap;

namespace Penedating.IoC.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            log4net.Config.XmlConfigurator.Configure();
            var useSecureCookie = ConfigurationManager.AppSettings["UseSecureCookie"] == "true";

            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IUserService>()
                                                 .Use<MongoUserService>();

                                             x.For<IUserProfileService>()
                                                 .Use<MongoUserProfileService>();

                                             x.For<IUserRepository>()
                                                 .Use<UserRepository>()
                                                    .Ctor<string>("connectionString").Is("mongodb://localhost/?safe=true")
                                                    .Ctor<string>("databaseName").Is("penedating");

                                             x.For<IUserProfileRepository>()
                                                .Use<UserProfileRepository>()
                                                   .Ctor<string>("connectionString").Is("mongodb://localhost/?safe=true")
                                                   .Ctor<string>("databaseName").Is("penedating");

                                             x.For<IUserAccessTokenProvider>()
                                                 .Use<MemcachedAccessTokenProvider>()
                                                    .Ctor<string>("cookieName").Is("penedating-auth")
                                                    .Ctor<bool>("useSecureCookie").Is(useSecureCookie);

                                             var memcachedConfig = new MemcachedClientConfiguration
                                                                       {
                                                                           Protocol = MemcachedProtocol.Binary,
                                                                           Transcoder = new NewtonsoftTranscoder()
                                                                       };
                                             memcachedConfig.AddServer("localhost", 11211);

                                             x.For<IMemcachedClient>().Singleton().Use(new MemcachedClient(memcachedConfig));
                                         });

            return ObjectFactory.Container;
        }
    }
}