using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, Teamdto>().ReverseMap();
        }
    }
}
