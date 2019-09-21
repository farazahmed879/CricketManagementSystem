 
Create Procedure usp_GetAllBowlingStatistics(
paramTeamId AS INT,
paramSeason As Int,
paramOvers As Int,
paramMatchType As Int,
paramTournamentId As Int,
paramMatchseriesId As Int,
paramPlayerRoleId As Int)
BEGIN
	SELECT  count(PlayerScores.MatchId) as `TotalMatch`,
			count(case when PlayerScores.Overs != null and PlayerScores.Overs != 0 then 1 else null end) as `TotalInnings`,
			sum(PlayerScores.Overs) as `TotalOvers`,
			sum(PlayerScores.Ball_Runs) as `TotalBallRuns`,
			sum(PlayerScores.Wickets) as `TotalWickets`,
			sum(PlayerScores.Maiden) as `TotalMaidens`,
			CASE WHEN 
                                sum(PlayerScores.Wickets) IS NULL OR sum(PlayerScores.Wickets) = 0 
				THEN null
				ELSE CAST(
                                      SUM(Cast(PlayerScores.Ball_Runs as DECIMAL(10,6))) /
				      sum(Cast(PlayerScores.Wickets as DECIMAL(10,6)))
				AS DECIMAL(10,6))
				END As `BowlingAvg`,
			Case When
					sum(PlayerScores.overs) = 0 OR sum(PlayerScores.overs) is null
					Then null
					Else CAST(
			cast(
			sum(cast(PlayerScores.ball_runs as DECIMAL(10,6))) / 
			sum(cast(PlayerScores.overs as DECIMAL(10,6))) as DECIMAL(10,6)) 
			AS DECIMAL(10,2)
			)
			End AS `economy`,

		    count(Case When PlayerScores.Wickets >=5 Then 1 Else Null End) As `FiveWickets`,
			sum(PlayerScores.Catches) as `TotalCatches`,
		 	sum PlayerScores.RunOut) as `TotalRunOuts`,
			sum(PlayerScores.Stump) as `TotalStumps`,
			Players.FileName AS `Image`,
			Players.Player_Name AS `PlayerName`
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
	left join MatchSeries On Matches.MatchSeriesId = MatchSeries.MatchSeriesId
	left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	
	
	WHERE (paramTeamId Is NUll or Players.TeamId = paramTeamId or Matches.OppponentTeamId = @paramTeamId ) And 
		  (paramSeason IS NUll OR Matches.Season = paramSeason)	And
		  (paramOvers IS NUll OR Matches.MatchOvers = paramOvers)And
		  (paramMatchType IS NULL OR Matches.MatchTypeId = paramMatchType) And 
		  (paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = paramMatchseriesId) And 
		  (paramTournamentId IS NUll OR Tournaments.TournamentId = paramTournamentId) And
		  (paramPlayerRoleId IS NUll OR PlayerRole.PlayerRoleId = paramPlayerRoleId) AND
		  (Players.IsDeactivated != 1) and 
		  (Players.IsGuestorRegistered != "Guest" or Players.IsGuestorRegistered is null)
	
	GROUP BY PlayerScores.PlayerId,
			Players.Player_Name,
			PlayerRole.Name,
			Players.FileName,
			Players.TeamId
END