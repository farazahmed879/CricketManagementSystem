using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class MatchSeries : Profile
    {
        public MatchSeries()
        {
            CreateMap<MatchSeries, MatchSeriesdto>().ReverseMap();
        }
    }
}
