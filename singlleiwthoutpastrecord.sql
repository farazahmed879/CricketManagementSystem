Alter PROCEDURE [usp_GetSinglePlayerStatistics]
@paramPlayerId AS INT
AS
BEGIN
	SELECT *,
		 CONCAT(data.TotalBallRuns,'/',data.MostWickets) AS 'BestBowling'
	FROM
	(
		SELECT  
				 COALESCE(count (PlayerScores.MatchId),0) as 'TotalMatch',
				 COALESCE(count (CASE WHEN PlayerScores.IsPlayedInning = 1 THEN 1 ELSE NULL END),0) as 'TotalInnings',
				 COALESCE(sum (PlayerScores.Bat_Runs),0) as 'TotalBatRuns',
				 COALESCE(sum (PlayerScores.Bat_Balls),0) as 'TotalBatBalls',
				 COALESCE(sum (PlayerScores.Four),0) as 'TotalFours',
				 COALESCE(sum(PlayerScores.Six),0) as 'TotalSixes',
				 COALESCE(count(case when HowOutId = 7 then 1 else null end),0) as 'TotalNotOut',
				 COALESCE(count(case when HowOutId = 2 then 1 else null end),0) as 'GetBowled',
				 COALESCE(count(case when HowOutId = 1 then 1 else null end),0) as 'GetCatch',
				 COALESCE(count(case when HowOutId = 3 then 1 else null end),0) as 'GetStump',
				 COALESCE(count(case when HowOutId = 4 then 1 else null end),0) as 'GetRunOut',
				 COALESCE(count(case when HowOutId = 6 then 1 else null end),0) as 'GetHitWicket',
				 COALESCE(count(case when HowOutId = 5 then 1 else null end),0) as 'GetLBW',
				 COALESCE(max (Bat_Runs),0) as 'BestScore',
				 COALESCE(max (Wickets),0) as 'MostWickets',		
				 COALESCE(COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END),0) AS 'NumberOf50s',
			   	COALESCE(COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END),0) AS 'NumberOf100s',
				Case When  COALESCE(sum(Bat_Balls),0) is null  OR  COALESCE(sum(Bat_Balls),0) = 0 
					THEN null
				    ELSE CAST(
								cast( COALESCE(sum (Bat_Runs),0) as float) * 100 / 
								CAST( COALESCE(sum(Bat_Balls),0) as float) AS numeric(36,2))
				END As 'StrikeRate',


				CASE WHEN (CAST( COALESCE(cOUNT(Case When IsPlayedInning ='1' Then 1 else null end),0) as float)) - 
						  (Cast( COALESCE(Count(case when HowOutId = 7 then 1 else null end),0) as float)) = 0
					THEN null
				    ELSE CAST(
								Cast( COALESCE(sum (Bat_Runs),0) as float) / 
								(cast( COALESCE(COUNT(Case When IsPlayedInning = 1 Then 1 else null end),0)as float)) - 
								(cast( COALESCE(COUNT (case when HowOutId = 7 then 1 else null end),0)as float))
							   AS numeric(36,2))
				END As 'BattingAverage',
				 COALESCE(sum (Overs),0) as 'TotalOvers',
				 COALESCE(sum (Ball_Runs),0) as 'TotalBallRuns',
				 COALESCE(sum (Wickets),0) as 'TotalWickets',
				 COALESCE(sum (Maiden),0) as 'TotalMaidens',
			
				CASE WHEN  COALESCE(sum(Wickets),0) is null OR  COALESCE(sum(Wickets),0) = 0 
					THEN null
					ELSE CAST((CAST( COALESCE(SUM(Ball_Runs),0)as float) / CAST( COALESCE(sum(Wickets),0) as float)) AS numeric(36,2))
					END As 'BowlingAvg',

				CASE WHEN ( COALESCE(sum(Overs),0)) IS NULL OR (COALESCE(sum(Overs),0)) = 0 
					THEN null
				ELSE CAST((CAST( COALESCE(SUM(Ball_Runs),0)as float) / CAST( COALESCE(sum(Overs),0) as float)) AS numeric(36,2))
					END As 'Economy',
									
			     COALESCE(count(Case When Wickets >=5 Then 1 Else Null End),0) As 'FiveWickets',
				 COALESCE(sum (Catches),0) as 'OnFieldCatch',
			 	 COALESCE(sum (RunOut),0) as 'OnFieldRunOut',
				 COALESCE(sum (Stump),0) as 'OnFieldStump',
				Players.Player_Name AS 'PlayerName',
				Players.TeamId As 'TeamId',					
				Teams.Team_Name As 'TeamName',
				Players.PlayerLogo As 'PlayerImage',
				Convert(varchar(10), Players.DOB) as 'DOB',
				BattingStyle.Name As 'BattingStyle',
				BowlingStyle.Name As 'BowlingStyle',
				PlayerRole.Name As 'PlayerRole'
		
		FROM Players
		left join PlayerScores ON PlayerScores.PlayerId = Players.PlayerId
		left join Teams ON Players.TeamId = Teams.TeamId
		left join Matches ON PlayerScores.MatchId = Matches.MatchId
		left join BattingStyle On Players.BattingStyleId = BattingStyle.BattingStyleId
		left join BowlingStyle On Players.BowlingStyleId = BowlingStyle.BowlingStyleId
		left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	
		WHERE Players.PlayerId = @paramPlayerId
		GROUP BY --PlayerScores.PlayerId,
				 Players.Player_Name,
				 --Players.PlayerRoleId,
				 --Players.BowlingStyleId,
				 --Players.BattingStyleId,
				 Players.TeamId,			 
				 Teams.Team_Name,
                 Players.DOB,
				 Players.PlayerLogo,
				 BattingStyle.Name,
				 BowlingStyle.Name,
				 PlayerRole.Name
				 --PlayerPastRecord.TotalMatch,
				 --PlayerPastRecord.TotalInnings,
				 --PlayerPastRecord.TotalBatRuns,
				 --PlayerPastRecord.TotalBatBalls,
				 --PlayerPastRecord.TotalFours,
				 --PlayerPastRecord.TotalSixes,
				 --PlayerPastRecord.TotalNotOut,
				 --PlayerPastRecord.GetBowled,
				 --PlayerPastRecord.GetCatch,
				 --PlayerPastRecord.GetStump,
				 --PlayerPastRecord.GetRunOut,
				 --PlayerPastRecord.GetHitWicket,
				 --PlayerPastRecord.GetLBW,
				 --PlayerPastRecord.TotalFours,
				 --PlayerPastRecord.TotalSixes,
				 --PlayerPastRecord.TotalNotOut,
				 --PlayerPastRecord.GetBowled,
				 --PlayerPastRecord.GetCatch,
				 --PlayerPastRecord.GetStump,
				 --PlayerPastRecord.GetRunOut,
				 --PlayerPastRecord.GetHitWicket,
				 --PlayerPastRecord.GetLBW,
				 --PlayerPastRecord.NumberOf50s,
				 --PlayerPastRecord.NumberOf100s,
				 --PlayerPastRecord.FiveWickets,
				 --PlayerPastRecord.OnFieldCatch,
				 --PlayerPastRecord.OnFieldRunOut,
				 --PlayerPastRecord.OnFieldStump,
				 --PlayerPastRecord.DoBowled,
				 --PlayerPastRecord.DoCatch,
				 --PlayerPastRecord.DoHitWicket,
				 --PlayerPastRecord.DoLBW,
				 --PlayerPastRecord.TotalOvers,
				 --PlayerPastRecord.DoStump,
				 --PlayerPastRecord.TotalBallRuns,
				 --PlayerPastRecord.TotalWickets,
				 --PlayerPastRecord.TotalMaidens

	) AS data
END
GO
