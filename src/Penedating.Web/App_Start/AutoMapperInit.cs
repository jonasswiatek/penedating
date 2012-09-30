using System.Web.Mvc;
using AutoMapper;
using Penedating.IoC.DependencyResolution;
using Penedating.Service.Model;
using Penedating.Web.Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Penedating.Web.App_Start.AutoMapperInit), "Start")]

namespace Penedating.Web.App_Start
{
    public static class AutoMapperInit
    {
        public static void Start()
        {
            Mapper.CreateMap<LoginModel, UserCredentials>()
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Password, b => b.MapFrom(c => c.Password));

            Mapper.CreateMap<UserCreateModel, UserCredentials>()
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Password, b => b.MapFrom(c => c.Password));

            Mapper.CreateMap<UserCreateModel, UserProfile>()
                .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                .ForMember(a => a.Address, b => b.MapFrom(c => c));

            Mapper.CreateMap<UserCreateModel, Address>()
                .ForMember(a => a.Street, b => b.MapFrom(c => c.StreetAddress))
                .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));
        }
    }
}