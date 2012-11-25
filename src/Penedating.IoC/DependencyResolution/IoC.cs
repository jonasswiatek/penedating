using System;
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
using Penedating.Service.RestApiService;
using StructureMap;

namespace Penedating.IoC.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            log4net.Config.XmlConfigurator.Configure();
            var useSecureCookie = ConfigurationManager.AppSettings["UseSecureCookie"] == "true";

            var externalPartners = new List<Uri>()
                                       {
                                           new Uri("http://173.203.86.196/ssase12/services/users"),
                                           new Uri("http://173.203.81.201/ssase12/services/users"),
                                           new Uri("http://173.203.82.223/ssase12/services/users"),
                                           new Uri("http://173.203.84.108/ssase12/services/users"),
                                           new Uri("http://173.203.84.157/ssase12/services/users"),
                                           new Uri("http://173.203.84.203/ssase12/services/users"),
                                           new Uri("http://173.203.84.207/ssase12/services/users"),
                                           new Uri("http://173.203.84.251/ssase12/services/users"),
                                           new Uri("http://173.203.78.105/ssase12/services/users"),
                                           new Uri("http://184.106.176.150/ssase12/services/users"),
                                           new Uri("http://184.106.134.110/ssase12/services/users")
                                       };

            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IExternalProfilesService>()
                                              .Use<RestApiExternalProfilesService>()
                                              .Ctor<IEnumerable<Uri>>("partners")
                                              .Is(externalPartners);

                                             x.For<IUserService>()
                                                 .Use<MongoUserService>();

                                             x.For<IUserProfileService>()
                                                 .Use<MongoUserProfileService>();

                                             x.For<IHugService>()
                                              .Use<MongoHugService>();

                                             x.For<IUserRepository>()
                                                 .Use<UserRepository>()
                                                    .Ctor<string>("connectionString").Is("mongodb://localhost/?safe=true")
                                                    .Ctor<string>("databaseName").Is("penedating");

                                             x.For<IUserProfileRepository>()
                                                .Use<UserProfileRepository>()
                                                   .Ctor<string>("connectionString").Is("mongodb://localhost/?safe=true")
                                                   .Ctor<string>("databaseName").Is("penedating");

                                             x.For<IHugRepository>()
                                               .Use<HugRepository>()
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