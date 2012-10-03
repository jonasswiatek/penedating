using System;
using AutoMapper;
using Penedating.Service.Model;
using Penedating.Service.MongoService.App_Start;
using System.Linq;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AutoMapperInit), "Start")]

namespace Penedating.Service.MongoService.App_Start
{
    public static class AutoMapperInit
    {
        public static void Start()
        {
            Mapper.CreateMap<Data.MongoDB.Model.User, Model.UserAccessToken>()
                .ForMember(a => a.Ticket, b => b.MapFrom(c => c.UserID))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email));

            Mapper.CreateMap<Data.MongoDB.Model.UserProfile, Model.UserProfile>()
                .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                .ForMember(a => a.Address, b => b.MapFrom(c => c.Address))
                .ForMember(a => a.Hobbies, b => b.MapFrom(c => c.Hobbies))
                .ForMember(a => a.Interests, b => b.MapFrom(c => c.Interests.Select(a => Enum.Parse(typeof(Interest), a))));

            Mapper.CreateMap<Data.MongoDB.Model.Address, Model.Address>()
                .ForMember(a => a.Street, b => b.MapFrom(c => c.StreetAddress))
                .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));

            Mapper.CreateMap<Service.Model.UserProfile, Data.MongoDB.Model.UserProfile>()
                .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                .ForMember(a => a.Address, b => b.MapFrom(c => c.Address))
                .ForMember(a => a.Hobbies, b => b.MapFrom(c => c.Hobbies))
                .ForMember(a => a.Interests, b => b.MapFrom(c => c.Interests.Select(a => a.ToString())));

            Mapper.CreateMap<Service.Model.Address, Data.MongoDB.Model.Address>()
                .ForMember(a => a.StreetAddress, b => b.MapFrom(c => c.Street))
                .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));
        }
    }
}