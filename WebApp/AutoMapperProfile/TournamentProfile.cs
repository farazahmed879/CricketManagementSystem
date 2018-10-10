using AutoMapper;
using CricketApp.Domain;
using WebApp.ViewModels;

namespace WebApp.AutoMapperProfile
{
    public class TournamentProfile : Profile
    {
        public TournamentProfile()
        {
            CreateMap<Tournament, Tournamentdto>().ReverseMap();
        }
    }
}
