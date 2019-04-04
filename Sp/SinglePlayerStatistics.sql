Alter PROCEDURE [usp_GetSinglePlayerStatistics]
@paramPlayerId AS INT
AS
BEGIN
		SELECT  * 
			from(
				select
				COALESCE( PlayerPastRecord.TotalMatch,0) + COALESCE(count (PlayerScores.MatchId),0) as 'TotalMatch',
				COALESCE(PlayerPastRecord.TotalInnings,0) + COALESCE(count (CASE WHEN PlayerScores.IsPlayedInning = 1 THEN 1 ELSE NULL END),0) as 'TotalInnings',
				(COALESCE(PlayerPastRecord.TotalBatRuns,0) + COALESCE(sum (PlayerScores.Bat_Runs),0)) as 'TotalBatRuns',
				(COALESCE(PlayerPastRecord.TotalBatBalls,0) + COALESCE(sum (PlayerScores.Bat_Balls),0)) as 'TotalBatBalls',
				(COALESCE(PlayerPastRecord.TotalFours,0) + COALESCE(sum (PlayerScores.Four),0)) as 'TotalFours',
				(COALESCE(PlayerPastRecord.TotalSixes,0) + COALESCE(sum(PlayerScores.Six),0)) as 'TotalSixes',
				(COALESCE(PlayerPastRecord.TotalNotOut,0) + COALESCE(count(case when HowOutId = 7 or howOutId = 8 then 1 else null end),0)) as 'TotalNotOut',
				(COALESCE(PlayerPastRecord.GetBowled,0) + COALESCE(count(case when HowOutId = 2 then 1 else null end),0)) as 'GetBowled',
				(COALESCE(PlayerPastRecord.GetCatch,0) + COALESCE(count(case when HowOutId = 1 then 1 else null end),0)) as 'GetCatch',
				(COALESCE(PlayerPastRecord.GetStump,0) + COALESCE(count(case when HowOutId = 3 then 1 else null end),0)) as 'GetStump',
				(COALESCE(PlayerPastRecord.GetRunOut,0) + COALESCE(count(case when HowOutId = 4 then 1 else null end),0)) as 'GetRunOut',
				(COALESCE(PlayerPastRecord.GetHitWicket,0)+ COALESCE(count(case when HowOutId = 6 then 1 else null end),0)) as 'GetHitWicket',
				(COALESCE(PlayerPastRecord.GetLBW,0) + COALESCE(count(case when HowOutId = 5 then 1 else null end),0)) as 'GetLBW',
				(
				select top 1 (Ball_Runs) FROM PlayerScores where Wickets = (select max(wickets) from playerScores where playerId = @paramPlayerId)
				) 
				as 'BestBowlingFigureRuns',
				
				
				case when (COALESCE(PlayerPastRecord.BestScore,0) >= COALESCE(max (Bat_Runs),0))
				then COALESCE(PlayerPastRecord.BestScore,0)
				else COALESCE(max (Bat_Runs),0)
				End as 'HightScore',
				COALESCE(max (Wickets),0) as 'MostWickets',		
				(COALESCE(PlayerPastRecord.NumberOf50s,0) + COALESCE(COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END),0)) AS 'NumberOf50s',
				(COALESCE(PlayerPastRecord.NumberOf100s,0) + COALESCE(COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END),0)) AS 'NumberOf100s',
				Case When (COALESCE(PlayerPastRecord.TotalBatBalls,0) + COALESCE(sum(Bat_Balls),0)) is null  OR (COALESCE(PlayerPastRecord.TotalBatBalls,0) + COALESCE(sum(Bat_Balls),0)) = 0 
					THEN null
				    ELSE CAST(
								cast((COALESCE(PlayerPastRecord.TotalBatRuns,0) + COALESCE(sum (Bat_Runs),0)) as float) * 100 / 
								CAST((COALESCE(PlayerPastRecord.TotalBatBalls,0) + COALESCE(sum(Bat_Balls),0)) as float) AS numeric(36,2))
				END As 'StrikeRate',


				CASE WHEN (CAST((COALESCE(PlayerPastRecord.TotalInnings,0) + COALESCE(cOUNT(Case When IsPlayedInning ='1' Then 1 else null end),0)) as float)) - 
						  (Cast((COALESCE(PlayerPastRecord.TotalNotOut,0) + COALESCE(Count(case when HowOutId = 7 or howOutId = 8 then 1 else null end),0)) as float)) = 0
					THEN null
				    ELSE CAST(
								(Cast((COALESCE(PlayerPastRecord.TotalBatRuns,0) + COALESCE(sum (Bat_Runs),0)) as float)) / 
								((cast((COALESCE(PlayerPastRecord.TotalInnings,0) + COALESCE(COUNT(Case When IsPlayedInning = 1 Then 1 else null end),0))as float)) - 
								(cast((COALESCE(PlayerPastRecord.TotalNotOut,0) + COALESCE(COUNT (case when HowOutId = 7 or howOutId= 8 then 1 else null end),0))as float)))
							   AS numeric(36,2))
				END As 'BattingAverage',
				(cast(round(COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum (Overs),0),1) as numeric(36,1))) as 'TotalOvers',
				(COALESCE(PlayerPastRecord.TotalBallRuns,0) + COALESCE(sum (Ball_Runs),0)) as 'TotalBallRuns',
				(COALESCE(PlayerPastRecord.TotalWickets,0) + COALESCE(sum (Wickets),0)) as 'TotalWickets',
				(COALESCE(PlayerPastRecord.TotalMaidens,0) + COALESCE(sum (Maiden),0)) as 'TotalMaidens',
				(COALESCE(PlayerPastRecord.DoBowled,0)) as 'DoBowled',
				(COALESCE(PlayerPastRecord.DoCatch,0)) as 'DoCatch',
				(COALESCE(PlayerPastRecord.DoHitWicket,0)) as 'DoHitWicket',
				(COALESCE(PlayerPastRecord.DoLBW,0)) as 'DoLBW',
				(COALESCE(PlayerPastRecord.DoStump,0)) as 'DoStump',
				CASE WHEN (COALESCE(PlayerPastRecord.TotalWickets,0) + COALESCE(sum(Wickets),0)) is null OR (COALESCE(PlayerPastRecord.TotalWickets,0) + COALESCE(sum(Wickets),0)) = 0 
					THEN null
					ELSE CAST((CAST((COALESCE(PlayerPastRecord.TotalBallRuns,0) + COALESCE(SUM(Ball_Runs),0))as float) / CAST((COALESCE(PlayerPastRecord.TotalWickets,0) + COALESCE(sum(Wickets),0)) as float)) AS numeric(36,2))
					END As 'BowlingAvg',

				--CASE WHEN (COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum(Overs),0)) IS NULL OR (COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum(Overs),0)) = 0 
				--	THEN null
				--ELSE CAST((CAST((COALESCE(PlayerPastRecord.TotalBallRuns,0) + COALESCE(SUM(Ball_Runs),0))as float) / CAST((COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum(Overs),0)) as float)) AS numeric(36,2))
				--	END As 'Economy',

					CASE WHEN (COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum(Overs),0)) IS NULL OR (COALESCE(PlayerPastRecord.TotalOvers,0) + COALESCE(sum(Overs),0)) = 0 
					THEN null
				ELSE 
						cast(((COALESCE(PlayerPastRecord.TotalBallRuns,0) + COALESCE(SUM(Ball_Runs),0)) / (floor(COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0)) * 6 + cast(replace(((COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0))  - floor(COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0))),'.','') as int)))*6  as numeric(36,2))		
				END As 'Economy',
				floor(COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0)) * 6 + cast(replace(((COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0))  - floor(COALESCE(sum(Overs),0) + COALESCE(PlayerPastRecord.TotalOvers,0))),'.','') as int) as 'TotalBalls',
			    (COALESCE(PlayerPastRecord.FiveWickets,0) + COALESCE(count(Case When Wickets >=5 Then 1 Else Null End),0)) As 'FiveWickets',
				(COALESCE(PlayerPastRecord.OnFieldCatch,0) + COALESCE(sum (Catches),0)) as 'OnFieldCatch',
			 	(COALESCE(PlayerPastRecord.OnFieldRunOut,0) + COALESCE(sum (RunOut),0)) as 'OnFieldRunOut',
				(COALESCE(PlayerPastRecord.OnFieldStump,0) + COALESCE(sum (Stump),0)) as 'OnFieldStump',
				COALESCE(count (case when Overs != null and Overs != 0 then 1 else null end),0) as 'TotalBowlingInnings',
				Players.Player_Name AS 'PlayerName',
				Players.TeamId As 'TeamId',					
				Teams.Team_Name As 'TeamName',
				Players.[FileName] As 'PlayerImage',
				Players.DOB AS 'DOB',
				--Convert(varchar(10), Players.DOB) as 'DOB',
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
	
		WHERE Players.PlayerId = @paramPlayerId
		GROUP BY PlayerScores.PlayerId,
				 Players.Player_Name,
				 Players.TeamId,			 
				 Teams.Team_Name,
                 Players.DOB,
				 Players.[FileName],
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
				 PlayerPastRecord.TotalMaidens,
				 PlayerPastRecord.BestScore
				 ) 
				 as SinglePlayerStatistics
END
GO 




