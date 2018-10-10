using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class PlayerPastRecordProfile : Profile
    {
        public PlayerPastRecordProfile()
        {
            CreateMap<PlayerPastRecord, PlayerPastRecorddto>().ReverseMap();
        }
    }
}
