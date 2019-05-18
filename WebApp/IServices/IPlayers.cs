using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IPlayers
    {
        Task<PaginatedList<Playersdto>> GetAllPlayersList(DataTableAjaxPostModel model, int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId);
        List<PlayersDropDowndto> GetAllPlayers();
        Task<PlayerPastRecorddto> GetPlayerPastRecordByPlayerId(int? playerId);
    }
}
