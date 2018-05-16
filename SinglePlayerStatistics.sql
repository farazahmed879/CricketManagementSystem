Alter PROCEDURE [usp_GetMatchStatistics]
@paramPlayerId AS INT
AS
BEGIN
	SELECT *,
		  CONCAT(data.TotalBallRuns,'/',data.MostWickets) AS 'BestBowling'
	FROM
	(
		SELECT  count (PlayerScores.MatchId) as 'TotalMatch',
				count (IsPlayedInning) as 'TotalInnings',
				sum (Bat_Runs) as 'TotalBatRuns',
				sum (Bat_Balls) as 'TotalBatBalls',
				sum (Four) as 'TotalFours',
				sum (Six) as 'TotalSixes',
				count(case when HowOut = 'Not Out' then 1 else null end) as 'TotalNotOut',
				count(case when HowOut = 'Bowled' then 1 else null end) as 'TotalBowled',
				count(case when HowOut = 'Catch' then 1 else null end) as 'TotalCatch',
				count(case when HowOut = 'Stump' then 1 else null end) as 'TotalStump',
				count(case when HowOut = 'RunOut' then 1 else null end) as 'TotalRunOut',
				count(case when HowOut = 'Hit Wicket' then 1 else null end) as 'TotalHitWicket',
				count(case when HowOut = 'LBW' then 1 else null end) as 'TotalLBW',
				max (Bat_Runs) as 'BestScore',
				max (Wickets) as 'MostWickets',		
				COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END) AS 'NumberOf50s',
				COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END) AS 'NumberOf100s',
				cast(sum (cast (Bat_Runs as float)) *100 / sum (cast (Bat_Balls as float)) as float)  as 'StrikeRate',
				CASE WHEN COUNT(cast (Case When IsPlayedInning ='1' Then 1 else null end as float)) - 
						  COUNT (cast (case when HowOut = 'Not Out' then 1 else null end as float)) = 0
					THEN 'N/A'
				    ELSE CAST(
								sum(cast (Bat_Runs as float)) / 
								(cast(COUNT(Case When IsPlayedInning ='1' Then 1 else null end)as float)) - 
								(cast (COUNT (case when HowOut = 'Not Out' then 1 else null end)as float))
							   AS VARCHAR(20))
				END As 'BattingAverage',
				sum (Overs) as 'TotalOvers',
				sum (Ball_Runs) as 'TotalBallRuns',
				sum (Wickets) as 'TotalWickets',
				sum (Maiden) as 'TotalMaidens',
				CASE WHEN COUNT(Wickets) IS NULL OR COUNT(Wickets) = 0 
					THEN 'N/A'
					ELSE CAST((CAST(SUM(Ball_Runs)as float) / CAST(COUNT(Wickets) as float)) AS VARCHAR(20))
					END As 'BowlingAvg',
				cast(SUM (cast (Ball_Runs as float)) / SUM(cast (Overs as float)) as float) As 'Economy',
			    count(Case When Wickets >=5 Then 1 Else Null End) As 'FiveWickets',
				sum (Catches) as 'TotalCatches',
			 	sum (RunOut) as 'TotalRunOuts',
				sum (Stump) as 'TotalStumps',
				Players.Player_Name AS 'PlayerName',
				Players.Role As 'Role',
				Players.TeamId As 'TeamId',
				Players.BowlingStyle As 'BowlingStyle',
				Players.BattingStyle As 'BattingStyle',			
				Teams.Team_Name As 'TeamName',
				Players.PlayerLogo As 'PlayerImage'
		
		FROM PlayerScores
		Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
		Inner join Teams ON Players.TeamId = Teams.TeamId
		WHERE PlayerScores.PlayerId = @paramPlayerId
		GROUP BY PlayerScores.PlayerId,
				 Players.Player_Name,
				 Players.Role,
				 Players.BowlingStyle,
				 Players.BattingStyle,
				 Players.TeamId,			 
				 Teams.Team_Name,
				 Players.PlayerLogo,
				 Players.MatchId
	) AS data
END

exec [usp_GetMatchStatistics] 1

