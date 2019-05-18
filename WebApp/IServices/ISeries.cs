using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ISeries
    {
         Task<PaginatedList<MatchSeriesdto>> GetAllSeries(DataTableAjaxPostModel model,int? page);
    }
}
