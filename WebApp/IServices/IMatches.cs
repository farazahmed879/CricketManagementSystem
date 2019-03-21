using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IMatches
    {

         Task<List<Matchdto>> GetAllMatches(int? teamId, int? matchTypeId,
                                               int? tournamentId, int? matchSeriesId,
                                                int? season, int? matchOvers, int? userId, int? page);
    }
}
