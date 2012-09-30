using AutoMapper;
using Penedating.Service.MongoService.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AutoMapperInit), "Start")]

namespace Penedating.Service.MongoService.App_Start
{
    public static class AutoMapperInit
    {
        public static void Start()
        {
            Mapper.CreateMap<Data.MongoDB.Model.User, Model.UserAccessToken>()
                .ForMember(a => a.Ticket, b => b.MapFrom(c => c.UserID));

            Mapper.CreateMap<Data.MongoDB.Model.UserProfile, Model.UserProfile>()
                .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                .ForMember(a => a.Address, b => b.MapFrom(c => c.Address));

            Mapper.CreateMap<Data.MongoDB.Model.Address, Model.Address>()
                .ForMember(a => a.Street, b => b.MapFrom(c => c.StreetAddress))
                .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));
        }
    }
}