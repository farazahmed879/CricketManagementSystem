Alter PROCEDURE [usp_GetAllBattingStatistics]
@paramTeamId AS INT,
@paramSeason As Int,
@paramOvers As Int,
@paramPosition As Int, 
@paramMatchType As Int,
@paramTournamentId As Int,
@paramMatchseriesId As Int,
@paramPlayerRoleId As Int,
@paramUserId AS int
AS
BEGIN
	SELECT  count (PlayerScores.MatchId) as 'TotalMatch',
			count (case when IsPlayedInning = 1 then 1 else null end) as 'TotalInnings',
			sum (Bat_Runs) as 'TotalBatRuns',
			sum (Bat_Balls) as 'TotalBatBalls',
			sum (Four) as 'TotalFours',
			sum (Six) as 'TotalSixes',
			COUNT(CASE WHEN Bat_Runs >= 50 THEN 1 ELSE NULL END) AS 'NumberOf50s',
			COUNT(CASE WHEN Bat_Runs >= 100 THEN 1 ELSE NULL END) AS 'NumberOf100s',
		
			--Calculating Batting Strike Rate
			CASE WHEN 
					Sum(Bat_Balls) = 0  OR Sum(Bat_Balls) is NULL
									THEN null
			ELSE CAST(
							Sum(cast (Bat_Runs as float)) *100 / 
							Sum(cast(Bat_Balls as float))
							   AS numeric(36,2)
							   )
			END As 'StrikeRate',

			CASE WHEN COUNT(cast (Case When IsPlayedInning ='1' Then 1 else null end as float)) - 
					  COUNT (cast (case when HowOutId = '7' or HowOutId = '8' then 1 else null end as float)) = 0
				THEN null
			    ELSE CAST(
							sum(cast (Bat_Runs as float)) / 
							(cast(COUNT(Case When IsPlayedInning ='1' Then 1 else null end)as float)) - 
							(cast (COUNT (case when HowOutId = '7' or HowOutId = '8'  then 1 else null end)as float))
						   AS numeric(36,2)
						   )
			END As 'BattingAverage',
			Players.Player_Name AS 'PlayerName',
			Players.TeamId As 'TeamId'

	
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	left join Tournaments On Matches.TournamentId = Tournaments.TournamentId
	left join MatchSeries On Matches.MatchSeriesId = MatchSeries.MatchSeriesId
	left join PlayerRole On Players.PlayerRoleId = PlayerRole.PlayerRoleId
	left join PlayerPastRecord On Players.PlayerId = PlayerPastRecord.PlayerId
	
	WHERE (@paramTeamId Is NUll or Players.TeamId = @paramTeamId or Matches.OppponentTeamId = @paramTeamId ) And 
		  (@paramSeason IS NUll OR Matches.Season = @paramSeason)	And
		  (@paramOvers IS NUll OR Matches.MatchOvers = @paramOvers)	And
		  (@paramPosition IS NULL OR PlayerScores.Position = @paramPosition) And 
		  (@paramMatchType IS NULL OR Matches.MatchTypeId = @paramMatchType) And 
		  (@paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = @paramMatchseriesId) And 
		  (@paramMatchseriesId IS NULL OR MatchSeries.MatchSeriesId = @paramMatchseriesId) And 
		  (@paramPlayerRoleId IS NUll OR PlayerRole.PlayerRoleId = @paramPlayerRoleId) And
		  (@paramUserId IS NUll OR Matches.UserId = @paramUserId) And
		    (Players.IsDeactivated != 1) and 
			(Players.IsGuestorRegistered != 'Guest' or Players.IsGuestorRegistered is null)
	
	GROUP BY PlayerScores.PlayerId,
			 Players.Player_Name,
			 PlayerRole.Name,
			 Players.TeamId
END
go