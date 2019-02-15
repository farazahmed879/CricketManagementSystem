Alter PROCEDURE [usp_OppTeamBowling]
@paramOppTeamId AS INT,
@paramMatchId AS INT
AS
BEGIN 
SELECT TOP 3
         Players.Player_Name AS 'OppTeamBowler',
         PlayerScores.Ball_Runs As 'Runs',
		 PlayerScores.Wickets As 'Wicket',
		  PlayerScores.Overs As 'Overs'
     FROM 
         PlayerScores
     INNER JOIN
         Players ON PlayerScores.PlayerId = Players.PlayerId
     INNER JOIN
         Teams ON Players.TeamId = Teams.TeamId
     INNER JOIN
         Matches ON PlayerScores.MatchId = Matches.MatchId
     WHERE 
         (Teams.TeamId = @paramOppTeamId)
         AND (Matches.MatchId = @paramMatchId)
     GROUP BY 
         Players.Player_Name,
         PlayerScores.Ball_Runs,
		 PlayerScores.Wickets,
		 PlayerScores.Overs
     ORDER BY
         MAX(Wickets) DESC 

END
go
