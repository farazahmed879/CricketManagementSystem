using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ITournaments
    {
         Task<PaginatedList<Tournamentdto>> GetAllTournaments(DataTableAjaxPostModel model,int? page);
    }
}
