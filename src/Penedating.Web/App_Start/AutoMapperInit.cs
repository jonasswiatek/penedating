using AutoMapper;
using Penedating.Service.Model;
using Penedating.Web.App_Start;
using Penedating.Web.Models;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AutoMapperInit), "Start")]

namespace Penedating.Web.App_Start
{
    public static class AutoMapperInit
    {
        public static void Start()
        {
            Mapper.CreateMap<LoginViewModel, UserCredentials>()
                  .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                  .ForMember(a => a.Password, b => b.MapFrom(c => c.Password));

            Mapper.CreateMap<CreateViewModel, UserCredentials>()
                  .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                  .ForMember(a => a.Password, b => b.MapFrom(c => c.Password));

            Mapper.CreateMap<CreateViewModel, UserProfile>()
                  .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                  .ForMember(a => a.Address, b => b.MapFrom(c => c));

            Mapper.CreateMap<CreateViewModel, Address>()
                  .ForMember(a => a.Street, b => b.MapFrom(c => c.StreetAddress))
                  .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                  .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));

            Mapper.CreateMap<UserProfile, ProfileViewModel>()
                  .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                  .ForMember(a => a.StreetAddress, b => b.MapFrom(c => c.Address.Street))
                  .ForMember(a => a.City, b => b.MapFrom(c => c.Address.City))
                  .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.Address.ZipCode))
                  .ForMember(a => a.Friendship, b => b.MapFrom(c => c.Interests.Contains(Interest.Friendship)))
                  .ForMember(a => a.Romance, b => b.MapFrom(c => c.Interests.Contains(Interest.Romance)));

            Mapper.CreateMap<UserProfile, ExternalProfile>()
                  .ForMember(a => a.name, b => b.MapFrom(c => c.Username))
                  .ForMember(a => a.hobbies, b => b.MapFrom(c => c.Hobbies))
                  .ForMember(a => a.url, b => b.UseValue(null));

            Mapper.CreateMap<UserProfile, ProfileListItem>()
                  .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                  .ForMember(a => a.Hobbies, b => b.MapFrom(c => c.Hobbies));

            Mapper.CreateMap<ExternalProfile, ProfileListItem>()
                  .ForMember(a => a.Username, b => b.MapFrom(c => c.name))
                  .ForMember(a => a.Hobbies, b => b.MapFrom(c => c.hobbies));

            Mapper.CreateMap<ProfileViewModel, Address>()
                  .ForMember(a => a.Street, b => b.MapFrom(c => c.StreetAddress))
                  .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                  .ForMember(a => a.ZipCode, b => b.MapFrom(c => c.ZipCode));

            Mapper.CreateMap<ProfileViewModel, UserProfile>()
                  .ForMember(a => a.Username, b => b.MapFrom(c => c.Username))
                  .ForMember(a => a.Address, b => b.MapFrom(c => c))
                  .ForMember(a => a.Hobbies, b => b.MapFrom(c => c.Hobbies));
        }
    }
}