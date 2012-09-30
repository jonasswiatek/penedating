using StructureMap;

namespace Penedating.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                                         {
                                             
                                         });

            return ObjectFactory.Container;
        }
    }
}