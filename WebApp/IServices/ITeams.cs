using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ITeams
    {
        Task<List<Teamdto>> GetAllTeams(string zone, string location, string name, int? page, int? userId);
        List<TeamDropDowndto> GetAllTeams();
    }
}
