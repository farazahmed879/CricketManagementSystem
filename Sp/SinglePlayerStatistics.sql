﻿Create PROCEDURE [usp_GetSinglePlayerStatistics]
@paramPlayerId AS INT,
@paramOvers AS INT
AS
BEGIN
	SELECT *,
		  CONCAT(data.TotalBallRuns,'/',data.MostWickets) AS 'BestBowling'
	FROM
	(
		SELECT  
				(COALESCE( PlayerPastRecord.TotalMatch,0) + count (PlayerScores.MatchId)) as 'TotalMatch',
				(COALESCE(PlayerPastRecord.TotalInnings,0) + count (IsPlayedInning)) as 'TotalInnings',
				(COALESCE(PlayerPastRecord.TotalBatRuns,0) + sum (PlayerScores.Bat_Runs)) as 'TotalBatRuns',
				(COALESCE(PlayerPastRecord.TotalBatBalls,0) + sum (Bat_Balls)) as 'TotalBatBalls',
				(COALESCE(PlayerPastRecord.TotalFours,0) + sum (Four)) as 'TotalFours',
				(COALESCE(PlayerPastRecord.TotalSixes,0) + sum(Six)) as 'TotalSixes',
				(COALESCE(PlayerPastRecord.TotalNotOut,0) + count(case when HowOutId = '14' then 1 else null end)) as 'TotalNotOut',
				(COALESCE(PlayerPastRecord.GetBowled,0) + count(case when HowOutId = '9' then 1 else null end)) as 'GetBowled',
				(COALESCE(PlayerPastRecord.GetCatch,0) + count(case when HowOutId = '8' then 1 else null end)) as 'GetCatch',
				(COALESCE(PlayerPastRecord.GetStump,0) + count(case when HowOutId = '10' then 1 else null end)) as 'GetStump',
				(COALESCE(PlayerPastRecord.GetRunOut,0) + count(case when HowOutId = '11' then 1 else null end)) as 'GetRunOut',
				(COALESCE(PlayerPastRecord.GetHitWicket,0)+ count(case when HowOutId = '13' then 1 else null end)) as 'GetHitWicket',
				(COALESCE(PlayerPastRecord.GetLBW,0) + count(case when HowOutId = '12' then 1 else null end)) as 'GetLBW',
				max (Bat_Runs) as 'BestScore',
				max (Wickets) as 'MostWickets',		
				(COALESCE(PlayerPastRecord.NumberOf50s,0) + COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END)) AS 'NumberOf50s',
				(COALESCE(PlayerPastRecord.NumberOf100s,0) + COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END)) AS 'NumberOf100s',
				Case When (COALESCE(PlayerPastRecord.TotalBatBalls,0) + sum(Bat_Balls)) is null  OR (COALESCE(PlayerPastRecord.TotalBatBalls,0) + sum(Bat_Balls)) = 0 
					THEN null
				    ELSE CAST(
								cast((COALESCE(PlayerPastRecord.TotalBatRuns,0) + sum (Bat_Runs)) as float) * 100 / 
								CAST((COALESCE(PlayerPastRecord.TotalBatBalls,0) + sum(Bat_Balls)) as float) AS numeric(36,2))
				END As 'StrikeRate',


				CASE WHEN (CAST((COALESCE(PlayerPastRecord.TotalInnings,0) + cOUNT(Case When IsPlayedInning ='1' Then 1 else null end)) as float)) - 
						  (Cast((COALESCE(PlayerPastRecord.TotalNotOut,0) + Count(case when HowOutId = '14' then 1 else null end)) as float)) = 0
					THEN null
				    ELSE CAST(
								Cast((COALESCE(PlayerPastRecord.TotalBatRuns,0) + sum (Bat_Runs)) as float) / 
								(cast((COALESCE(PlayerPastRecord.TotalInnings,0) + COUNT(Case When IsPlayedInning ='1' Then 1 else null end))as float)) - 
								(cast((COALESCE(PlayerPastRecord.TotalNotOut,0) + COUNT (case when HowOutId = '14' then 1 else null end))as float))
							   AS numeric(36,2))
				END As 'BattingAverage',
				(COALESCE(PlayerPastRecord.TotalOvers,0) + sum (Overs)) as 'TotalOvers',
				(COALESCE(PlayerPastRecord.TotalBallRuns,0) + sum (Ball_Runs)) as 'TotalBallRuns',
				(COALESCE(PlayerPastRecord.TotalWickets,0) + sum (Wickets)) as 'TotalWickets',
				(COALESCE(PlayerPastRecord.TotalMaidens,0) + sum (Maiden)) as 'TotalMaidens',
				(COALESCE(PlayerPastRecord.DoBowled,0)) as 'DoBowled',
				(COALESCE(PlayerPastRecord.DoCatch,0)) as 'DoCatch',
				(COALESCE(PlayerPastRecord.DoHitWicket,0)) as 'DoHitWicket',
				(COALESCE(PlayerPastRecord.DoLBW,0)) as 'DoLBW',
				(COALESCE(PlayerPastRecord.DoStump,0)) as 'DoStump',
				CASE WHEN (COALESCE(PlayerPastRecord.TotalWickets,0) + sum(Wickets)) IS NULL OR (COALESCE(PlayerPastRecord.TotalWickets,0) + sum(Wickets)) = 0 
					THEN null
					ELSE CAST((CAST((COALESCE(PlayerPastRecord.TotalBallRuns,0) + SUM(Ball_Runs))as float) / CAST((COALESCE(PlayerPastRecord.TotalInnings,0) + sum(Wickets)) as float)) AS numeric(36,2))
					END As 'BowlingAvg',

				CASE WHEN (COALESCE(PlayerPastRecord.TotalOvers,0) + sum(Overs)) IS NULL OR (COALESCE(PlayerPastRecord.TotalOvers,0) + sum(Overs)) = 0 
					THEN null
				ELSE CAST((CAST((COALESCE(PlayerPastRecord.TotalBallRuns,0) + SUM(Ball_Runs))as float) / CAST((COALESCE(PlayerPastRecord.TotalOvers,0) + sum(Overs)) as float)) AS numeric(36,2))
					END As 'Economy',
									
			    (COALESCE(PlayerPastRecord.FiveWickets,0) + count(Case When Wickets >=5 Then 1 Else Null End)) As 'FiveWickets',
				(COALESCE(PlayerPastRecord.OnFieldCatch,0) + sum (Catches)) as 'OnFieldCatches',
			 	(COALESCE(PlayerPastRecord.OnFieldRunOut,0) + sum (RunOut)) as 'OnFieldRunOut',
				(COALESCE(PlayerPastRecord.OnFieldStump,0) + sum (Stump)) as 'OnFieldStump',
				Players.Player_Name AS 'PlayerName',
				Players.TeamId As 'TeamId',					
				Teams.Team_Name As 'TeamName',
				Players.PlayerLogo As 'PlayerImage',
				Convert(varchar(10), Players.DOB) as 'DOB',
				Matches.MatchOvers As 'MatchOvers',
				BattingStyle.Name As 'BattingStyle',
				BowlingStyle.Name As 'BowlingStyle',
				PlayerRole.Name As 'PlayerRole'
		
		FROM Players
		left join PlayerPastRecord On Players.PlayerId = PlayerPastRecord.PlayerId
		left join PlayerScores ON PlayerScores.PlayerId = Players.PlayerId
		left join Teams ON Players.TeamId = Teams.TeamId
		left join Matches ON PlayerScores.MatchId = Matches.MatchId
		left join BattingStyle On Players.BattingStyleId = BattingStyle.BattingStyleId
		left join BowlingStyle On Players.BowlingStyleId = BowlingStyle.BowlingStyleId
		left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	
		WHERE Players.PlayerId = @paramPlayerId And (@paramOvers IS NUll OR Matches.MatchOvers = @paramOvers)
		GROUP BY PlayerScores.PlayerId,
				 Players.Player_Name,
				 Players.PlayerRoleId,
				 Players.BowlingStyleId,
				 Players.BattingStyleId,
				 Players.TeamId,			 
				 Teams.Team_Name,
                 Players.DOB,
				 Players.PlayerLogo,
				 Matches.MatchOvers,
				 BattingStyle.Name,
				 BowlingStyle.Name,
				 PlayerRole.Name,
				 PlayerPastRecord.TotalMatch,
				 PlayerPastRecord.TotalInnings,
				 PlayerPastRecord.TotalBatRuns,
				 PlayerPastRecord.TotalBatBalls,
				 PlayerPastRecord.TotalFours,
				 PlayerPastRecord.TotalSixes,
				 PlayerPastRecord.TotalNotOut,
				 PlayerPastRecord.GetBowled,
				 PlayerPastRecord.GetCatch,
				 PlayerPastRecord.GetStump,
				 PlayerPastRecord.GetRunOut,
				 PlayerPastRecord.GetHitWicket,
				 PlayerPastRecord.GetLBW,
				 PlayerPastRecord.TotalFours,
				 PlayerPastRecord.TotalSixes,
				 PlayerPastRecord.TotalNotOut,
				 PlayerPastRecord.GetBowled,
				 PlayerPastRecord.GetCatch,
				 PlayerPastRecord.GetStump,
				 PlayerPastRecord.GetRunOut,
				 PlayerPastRecord.GetHitWicket,
				 PlayerPastRecord.GetLBW,
				 PlayerPastRecord.NumberOf50s,
				 PlayerPastRecord.NumberOf100s,
				 PlayerPastRecord.FiveWickets,
				 PlayerPastRecord.OnFieldCatch,
				 PlayerPastRecord.OnFieldRunOut,
				 PlayerPastRecord.OnFieldStump,
				 PlayerPastRecord.DoBowled,
				 PlayerPastRecord.DoCatch,
				 PlayerPastRecord.DoHitWicket,
				 PlayerPastRecord.DoLBW,
				 PlayerPastRecord.TotalOvers,
				 PlayerPastRecord.DoStump,
				 PlayerPastRecord.TotalBallRuns,
				 PlayerPastRecord.TotalWickets,
				 PlayerPastRecord.TotalMaidens
	) AS data
END
GO