Alter PROCEDURE [usp_GetSinglePlayerStatistics]
@paramPlayerId AS INT,
@paramOvers AS INT
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
				count(case when HowOutId = '14' then 1 else null end) as 'TotalNotOut',
				count(case when HowOutId = '9' then 1 else null end) as 'TotalBowled',
				count(case when HowOutId = '8' then 1 else null end) as 'TotalCatch',
				count(case when HowOutId = '10' then 1 else null end) as 'TotalStump',
				count(case when HowOutId = '11' then 1 else null end) as 'TotalRunOut',
				count(case when HowOutId = '13' then 1 else null end) as 'TotalHitWicket',
				count(case when HowOutId = '12' then 1 else null end) as 'TotalLBW',
				max (Bat_Runs) as 'BestScore',
				max (Wickets) as 'MostWickets',		
				COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END) AS 'NumberOf50s',
				COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END) AS 'NumberOf100s',
				cast(sum (cast (Bat_Runs as float)) *100 / sum (cast (Bat_Balls as float)) as float)  as 'StrikeRate',
				CASE WHEN COUNT(cast (Case When IsPlayedInning ='1' Then 1 else null end as float)) - 
						  COUNT (cast (case when HowOutId = '14' then 1 else null end as float)) = 0
					THEN 'N/A'
				    ELSE CAST(
								sum(cast (Bat_Runs as float)) / 
								(cast(COUNT(Case When IsPlayedInning ='1' Then 1 else null end)as float)) - 
								(cast (COUNT (case when HowOutId = '14' then 1 else null end)as float))
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
				Players.TeamId As 'TeamId',					
				Teams.Team_Name As 'TeamName',
				Players.PlayerLogo As 'PlayerImage',
				Matches.MatchOvers As 'MatchOvers'
				--BattingStyle.Name As 'BattingStyle',
				--BowlingStyle.Name As 'BowlingStyle',
				--PlayerRole.Name As 'PlayerRole'
		
		FROM PlayerScores
		Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
		Inner join Teams ON Players.TeamId = Teams.TeamId
		Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
		--left join BattingStyle On Players.BattingStyleId = BattingStyle.BattingStyleId
		--left join BowlingStyle On Players.BowlingStyleId = BowlingStyle.BowlingStyleId
		--left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	
		WHERE PlayerScores.PlayerId = @paramPlayerId And (@paramOvers IS NUll OR Matches.MatchOvers = @paramOvers)
		GROUP BY PlayerScores.PlayerId,
				 Players.Player_Name,
				 Players.PlayerRoleId,
				 Players.BowlingStyleId,
				 Players.BattingStyleId,
				 Players.TeamId,			 
				 Teams.Team_Name,
				 Players.PlayerLogo,
				 Matches.MatchOvers
				 --BattingStyle.Name,
				 --BowlingStyle.Name,
				 --PlayerRole.Name
				 
	) AS data
END
exec [usp_GetSinglePlayerStatistics] 1, 0

select * from Players


delete from matches

select * from PlayerScores

select * from TeamScores