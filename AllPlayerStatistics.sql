Alter PROCEDURE [usp_GetAllPlayerStatistics]
@paramTeamId AS INT,
@paramSeason As Int,
@paramOvers As Int,
@paramPosition As Int, 
@paramMatchType As Int,
@paramTournamentId As Int,
@paramOpponentTeamId As Int
AS
BEGIN
	SELECT  count (PlayerScores.MatchId) as 'TotalMatch',
			count (IsPlayedInning) as 'TotalInnings',
			sum (Bat_Runs) as 'TotalBatRuns',
			sum (Bat_Balls) as 'TotalBatBalls',
			sum (Four) as 'TotalFours',
			sum (Six) as 'TotalSixes',
			COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END) AS 'NumberOf50s',
			COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END) AS 'NumberOf100s',
		
			--Calculating Batting Strike Rate
			CASE WHEN 
					Sum(cast (Bat_Balls as float)) = 0  OR Sum(cast (Bat_Balls as float)) = NULL
									THEN 'N/A'
			ELSE CAST(
							Sum(cast (Bat_Runs as float)) *100 / 
							Sum(cast(Bat_Balls as float))
							   AS VARCHAR(20)
							   )
			END As 'StrikeRate',

			CASE WHEN COUNT(cast (Case When IsPlayedInning ='1' Then 1 else null end as float)) - 
					  COUNT (cast (case when HowOutId = '14' then 1 else null end as float)) = 0
				THEN 'N/A'
			    ELSE CAST(
							sum(cast (Bat_Runs as float)) / 
							(cast(COUNT(Case When IsPlayedInning ='1' Then 1 else null end)as float)) - 
							(cast (COUNT (case when HowOutId = '14' then 1 else null end)as float))
						   AS VARCHAR(20)
						   )
			END As 'BattingAverage',
			sum (Overs) as 'TotalOvers',
			sum (Ball_Runs) as 'TotalBallRuns',
			sum (Wickets) as 'TotalWickets',
			sum (Maiden) as 'TotalMaidens',
			CASE WHEN COUNT(Wickets) IS NULL OR COUNT(Wickets) = 0 
				THEN 'N/A'
				ELSE CAST((CAST(SUM(Ball_Runs)as float) / CAST(COUNT(Wickets) as float)) AS VARCHAR(20))
				END As 'BowlingAvg',
			
			--Economy Rate
			Case When
					sum(cast(overs as float)) = 0 OR sum(cast(overs as float)) = null
					Then 'N/A'
					Else CAST(
			cast(sum (cast (ball_runs as float)) / sum(cast (overs as float)) as float) 
			as Varchar(20)
			)
			End AS 'economy',

		    count(Case When Wickets >=5 Then 1 Else Null End) As 'FiveWickets',
			sum (Catches) as 'TotalCatches',
		 	sum (RunOut) as 'TotalRunOuts',
			sum (Stump) as 'TotalStumps',
	
			PlayerRole.Name As 'PlayerRole',
			Players.Player_Name AS 'PlayerName',
			Players.TeamId As 'TeamId',		
			Teams.Team_Name As 'TeamName'

	
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	Inner join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
	
	
	WHERE (@paramTeamId Is NUll or Players.TeamId = @paramTeamId) And 
		  (@paramSeason IS NUll OR Matches.Season = @paramSeason)	And
		  (@paramOvers IS NUll OR Matches.MatchOvers = @paramOvers)	And
		  (@paramPosition IS NULL OR PlayerScores.Position = @paramPosition) And 
		  (@paramMatchType IS NULL OR Matches.MatchTypeId = @paramMatchType) And 
		  (@paramTournamentId IS NUll OR Tournaments.TournamentId = @paramTournamentId) And
		  (@paramOpponentTeamId IS NUll OR Matches.OppponentTeamId = @paramOpponentTeamId) 
	
	GROUP BY PlayerScores.PlayerId,
			Players.Player_Name,
			 PlayerRole.Name,
			 Players.TeamId,
			 Teams.Team_Name
END



execute [usp_GetAllPlayerStatistics] null,null,null,null,null,null,null


