using System.Web.Mvc;
using Penedating.Web.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof (Penedating.Web.App_Start.StructuremapMvc), "Start")]

namespace Penedating.Web.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}