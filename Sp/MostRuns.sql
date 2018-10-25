Create PROCEDURE [usp_GetMostRuns]
@paramTeamId AS INT,
@paramSeason As Int,
@paramOvers As Int,
@paramPosition As Int, 
@paramMatchType As Int,
@paramTournamentId As Int,
@paramMatchseriesId As Int,
@paramPlayerRoleId As Int
AS
BEGIN
	SELECT  top 10
			count (PlayerScores.MatchId) as 'TotalMatch',
			count (IsPlayedInning) as 'TotalInnings',
			sum (Bat_Runs) as 'TotalBatRuns',
			Players.Player_Name AS 'PlayerName'
			
	
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
	left join MatchSeries On Matches.MatchSeriesId = MatchSeries.MatchSeriesId
	left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	
	
	
	WHERE (@paramTeamId Is NUll or Players.TeamId = @paramTeamId or Matches.OppponentTeamId = @paramTeamId ) And 
		  (@paramSeason IS NUll OR Matches.Season = @paramSeason)	And
		  (@paramOvers IS NUll OR Matches.MatchOvers = @paramOvers)	And
		  (@paramPosition IS NULL OR PlayerScores.Position = @paramPosition) And 
		  (@paramMatchType IS NULL OR Matches.MatchTypeId = @paramMatchType) And 
		  (@paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = @paramMatchseriesId) And 
		  (@paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = @paramMatchseriesId) And 
		  (@paramPlayerRoleId IS NUll OR PlayerRole.PlayerRoleId = @paramPlayerRoleId)
	
	
	GROUP BY PlayerScores.PlayerId,
			Players.Player_Name,
			PlayerRole.Name,
			Players.TeamId
		--	PlayerScores.Bat_Runs

	order by sum(Bat_Runs) desc ;
END
go


