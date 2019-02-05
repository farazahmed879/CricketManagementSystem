using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ITournaments
    {
         Task<List<Tournamentdto>> GetAllTournaments(int? page, int? userId);
    }
}
