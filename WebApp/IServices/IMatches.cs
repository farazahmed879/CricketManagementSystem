using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IMatches
    {

         Task<PaginatedList<MatchListdto>> GetAllMatchesList(DataTableAjaxPostModel model, int? teamId, int? matchTypeId,
                                               int? tournamentId, int? matchSeriesId,
                                                int? season, int? matchOvers);
    }
}
