using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<Match, Matchdto>().ReverseMap();
        }
    }
}
