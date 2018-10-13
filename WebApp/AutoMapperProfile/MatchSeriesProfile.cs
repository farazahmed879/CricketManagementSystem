using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class MatchSeriesProfile : Profile
    {
        public MatchSeriesProfile()
        {
            CreateMap<MatchSeries, MatchSeriesdto>().ReverseMap();
        }
    }
}
