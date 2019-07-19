using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IGround
    {
         Task<PaginatedList<Grounddto>> GetAllGround(DataTableAjaxPostModel model,int? page);
    }
}
