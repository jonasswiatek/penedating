using System;
using System.Collections.Generic;
using System.Configuration;
using DR.Sleipner;
using DR.Sleipner.CacheConfiguration;
using DR.Sleipner.EnyimMemcachedProvider;
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
using log4net.Config;

namespace Penedating.IoC.DependencyResolution
{
    public static class IoC
    {
        public static SleipnerProxy<IExternalProfilesService> ExternalProfilesProxy;
        public static IMemcachedClient MemcachedClient;

        public static IContainer Initialize()
        {
            XmlConfigurator.Configure();
            var useSecureCookie = ConfigurationManager.AppSettings["UseSecureCookie"] == "true";

            var memcachedConfig = new MemcachedClientConfiguration
                                      {
                                          Protocol = MemcachedProtocol.Binary,
                                          Transcoder = new NewtonsoftTranscoder()
                                      };
            memcachedConfig.AddServer("localhost", 11211);

            MemcachedClient = new MemcachedClient(memcachedConfig);

            var externalPartners = new List<Uri>
                                       {
                                           new Uri("http://173.203.83.220/ssase12/services/users"),
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

            ExternalProfilesProxy = new SleipnerProxy<IExternalProfilesService>(
                new RestApiExternalProfilesService(externalPartners),
                new EnyimMemcachedProvider<IExternalProfilesService>(MemcachedClient)
                );

            ExternalProfilesProxy.Configure(a => a.ForAll().CacheFor(60));

            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IExternalProfilesService>()
                                              .Use(ExternalProfilesProxy.Object);

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

                                             x.For<IMemcachedClient>().Use(MemcachedClient);
                                         });

            return ObjectFactory.Container;
        }
    }
}