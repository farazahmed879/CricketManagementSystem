﻿
delimiter //
create PROCEDURE sp_GetAllBattingStatistics
(
In paramTeamId INT,
In paramSeason Int,
In paramOvers Int,
In paramPosition Int, 
In paramMatchType Int,
In paramTournamentId Int,
In paramMatchseriesId Int,
In paramPlayerRoleId Int,
In paramUserId int
)
BEGIN
	SELECT  count(PlayerScores.MatchId) as `TotalMatch`,
			count(case when IsPlayedInning = 1 then 1 else null end) as `TotalInnings`,
			sum(Bat_Runs) as `TotalBatRuns`,
			sum(Bat_Balls) as `TotalBatBalls`,
			sum(Four) as `TotalFours`,
			sum(Six) as `TotalSixes`,
			COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END) AS `NumberOf50s`,
			COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END) AS `NumberOf100s`,
		
		
			CASE WHEN 
					Sum(Bat_Balls) = 0  OR Sum(Bat_Balls) is NULL
									THEN null
			ELSE CAST(
							Sum(cast(Bat_Runs as DECIMAL(10,6))) *100 / 
							Sum(cast(Bat_Balls as DECIMAL(10,6)))
							   AS DECIMAL(10,2)
							   )
			END As `StrikeRate`,

			CASE WHEN COUNT(cast(Case When IsPlayedInning = 1 Then 1 else null end as decimal(10,2))) - 
					  COUNT (cast(case when HowOutId = 7 or HowOutId = 8 then 1 else null end as decimal(10,2))) = 0
				THEN null
			    ELSE CAST(
							sum(cast(Bat_Runs as DECIMAL(10,6))) / 
							(cast(COUNT(Case When IsPlayedInning =1 Then 1 else null end)as DECIMAL(10,6))) - 
							(cast(COUNT(case when HowOutId = 7 or HowOutId = 8  then 1 else null end)as DECIMAL(10,6)))
						   AS DECIMAL(10,2)
						   )
			END As `BattingAverage`,
			Players.Player_Name AS `PlayerName`,
			Players.FileName AS `Image`,
			Players.TeamId As `TeamId`

	
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
	left join MatchSeries On Matches.MatchSeriesId = MatchSeries.MatchSeriesId
	left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	left join PlayerPastRecord On Players.PlayerId = PlayerPastRecord.PlayerId
	
	WHERE (paramTeamId Is NUll or Players.TeamId = paramTeamId or Matches.OppponentTeamId = paramTeamId ) And 
		  (paramSeason IS NUll OR Matches.Season = paramSeason)	And
		  (paramOvers IS NUll OR Matches.MatchOvers = paramOvers)	And
		  (paramPosition IS NULL OR PlayerScores.Position = paramPosition) And 
		  (paramMatchType IS NULL OR Matches.MatchTypeId = paramMatchType) And 
		  (paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = paramMatchseriesId) And 
		  (paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = paramMatchseriesId) And 
		  (paramPlayerRoleId IS NUll OR PlayerRole.PlayerRoleId = paramPlayerRoleId) And
		  (paramUserId IS NUll OR Matches.UserId = paramUserId) And
		    (Players.IsDeactivated != 1) and 
			(Players.IsGuestorRegistered != "Guest" or Players.IsGuestorRegistered is null)
	
	GROUP BY PlayerScores.PlayerId,
			 Players.Player_Name,
			 PlayerRole.Name,
			 Players.TeamId,
			 Players.FileName;
END//
delimiter;