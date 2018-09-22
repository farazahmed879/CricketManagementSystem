Alter procedure [usp_Top10Runs]
@paramTeamId AS INT,
@paramTournamentId AS INT,
@season AS INT
 As begin

SELECT TOP(5) * FROM
(
       SELECT DISTINCT PlayerScores.Bat_Runs, Players.Player_Name
           FROM PlayerScores
		   Inner join Players on Players.PlayerId = PlayerScores.PlayerId
		   Inner join Teams on Teams.TeamId = PlayerScores.TeamId
		   Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
		   left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
		   where(@paramTeamId is Null or Teams.TeamId = @paramTeamId ) And
				(@paramTournamentId is Null or Tournaments.TournamentId = @paramTournamentId ) And
				(@season is Null or Matches.Season = @season )
) A
ORDER BY Bat_Runs DESC
end

execute [usp_Top10Runs] 10,3
select * from Tournaments

