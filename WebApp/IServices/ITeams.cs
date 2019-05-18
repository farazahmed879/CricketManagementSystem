using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ITeams
    {
        Task<PaginatedList<Teamdto>> GetAllTeamsList(DataTableAjaxPostModel model, string zone, string location, string name, int? page);
        List<TeamDropDowndto> GetAllTeams();
    }
}
