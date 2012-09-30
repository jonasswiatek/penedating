using Penedating.Data.MongoDB;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Service.Model.Contract;
using Penedating.Service.MongoService;
using StructureMap;

namespace Penedating.IoC.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             x.For<IUserService>().Use<MongoUserService>();
                                             x.For<IUserRepository>().Use<UserRepository>()
                                                 .Ctor<string>("connectionString").Is("mongodb://localhost/?safe=true")
                                                 .Ctor<string>("databaseName").Is("penedating");
                                         });

            return ObjectFactory.Container;
        }
    }
}