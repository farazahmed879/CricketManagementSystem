using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.IServices
{
    public interface IMatchSummary
    {
        Task<MatchDetails> GetBowler(int matchId, int teamId);
        Task<MatchDetails> GetBatsman(int matchId, int teamId);
        Task<MatchDetails> FirstInning(int matchId, int hometeamId, int oppTeamId);
        Task<MatchDetails> SecondInning(int matchId, int hometeamId, int oppTeamId);
    }
}
