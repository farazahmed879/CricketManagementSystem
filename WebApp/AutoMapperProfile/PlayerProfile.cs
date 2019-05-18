using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, MatchBatsman>().ReverseMap();
        }
    }
}
