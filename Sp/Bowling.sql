Alter PROCEDURE [usp_GetAllBowlingStatistics]
@paramTeamId AS INT,
@paramSeason As Int,
@paramOvers As Int,
@paramMatchType As Int,
@paramTournamentId As Int,
@paramMatchseriesId As Int,
@paramPlayerRoleId As Int
AS
BEGIN
	SELECT  count (PlayerScores.MatchId) as 'TotalMatch',
			count (IsPlayedInning) as 'TotalInnings',
			sum (Overs) as 'TotalOvers',
			sum (Ball_Runs) as 'TotalBallRuns',
			sum (Wickets) as 'TotalWickets',
			sum (Maiden) as 'TotalMaidens',
			CASE WHEN sum(Wickets) IS NULL OR sum(Wickets) = 0 
				THEN null
				ELSE CAST((CAST(SUM(Ball_Runs)as float) / CAST(sum(Wickets) as float)) AS numeric(36,2))
				END As 'BowlingAvg',
			
			--Economy Rate
			Case When
					sum(overs) = 0 OR sum(overs) is null
					Then null
					Else CAST(
			cast(sum (cast (ball_runs as float)) / sum(cast (overs as float)) as float) 
			AS numeric(36,2)
			)
			End AS 'economy',

		    count(Case When Wickets >=5 Then 1 Else Null End) As 'FiveWickets',
			sum (Catches) as 'TotalCatches',
		 	sum (RunOut) as 'TotalRunOuts',
			sum (Stump) as 'TotalStumps',
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
		  (@paramMatchType IS NULL OR Matches.MatchTypeId = @paramMatchType) And 
		  (@paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = @paramMatchseriesId) And 
		  (@paramTournamentId IS NUll OR Tournaments.TournamentId = @paramTournamentId) And
		  (@paramPlayerRoleId IS NUll OR PlayerRole.PlayerRoleId = @paramPlayerRoleId)
	
	GROUP BY PlayerScores.PlayerId,
			Players.Player_Name,
			PlayerRole.Name,
			 Players.TeamId
END
go