using System.Web.Mvc;
using Penedating.IoC.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof (Penedating.IoC.App_Start.StructuremapMvc), "Start")]

namespace Penedating.IoC.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = DependencyResolution.IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}