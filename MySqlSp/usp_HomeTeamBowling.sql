
delimiter //
create PROCEDURE usp_HomeTeamBowling
(In paramHomeTeamId INT,
In paramMatchId INT)
BEGIN 
SELECT 
         Players.Player_Name AS `HomeTeamBowler`,
         PlayerScores.Ball_Runs As `Runs`,
		 PlayerScores.Wickets As `Wicket`,
		 PlayerScores.Overs As `Overs`
     FROM 
         PlayerScores
     INNER JOIN
         Players ON PlayerScores.PlayerId = Players.PlayerId
     INNER JOIN
         Teams ON Players.TeamId = Teams.TeamId
     INNER JOIN
         Matches ON PlayerScores.MatchId = Matches.MatchId
     WHERE 
         (Teams.TeamId = @paramHomeTeamId)
         AND (Matches.MatchId = @paramMatchId)
     GROUP BY 
         Players.Player_Name,
         PlayerScores.Ball_Runs,
		 PlayerScores.Wickets,
		 PlayerScores.Overs
     ORDER BY
         MAX(Wickets) DESC 
         limit 3;
END //
delimiter ;
 