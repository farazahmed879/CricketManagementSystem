using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface ISeries
    {
         Task<List<MatchSeriesdto>> GetAllSeries(int? page, int? userId);
    }
}
