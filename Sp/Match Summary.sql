Alter PROCEDURE [usp_MatchSummary]
@paramHomeTeamId AS INT,
@paramOpponentTeamId AS INT,
@paramMatchId AS INT
AS
BEGIN
	SELECT  top 3
			Players.Player_Name AS 'HomeTeamBatsman',
			PlayerScores.Bat_Runs As 'HomeTeamRuns'	

	
	FROM PlayerScores
	Inner join Players ON PlayerScores.PlayerId = Players.PlayerId
	Inner join Teams ON Players.TeamId = Teams.TeamId
	Inner join Matches ON PlayerScores.MatchId = Matches.MatchId
	
	WHERE (Teams.TeamId = @paramHomeTeamId ) And 
		  (@paramMatchId IS NUll OR Matches.MatchId = @paramMatchId)

	GROUP BY 
			Players.Player_Name,
			PlayerScores.Bat_Runs
	order by max(Bat_Runs) desc 
END
go
exec [usp_MatchSummary] 1,3,1025


select top 3 max(Bat_runs) from PlayerScores

select * from Matches
