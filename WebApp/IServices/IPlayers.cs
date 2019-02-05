using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IPlayers
    {
         Task<List<Playersdto>> GetAllPlayers(int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId, int? page);
    }
}
