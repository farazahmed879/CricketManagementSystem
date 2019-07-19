﻿
delimiter //
create PROCEDURE usp_HomeTeamBatting
(In paramHomeTeamId INT,
In paramMatchId INT)
BEGIN 
SELECT 
         Players.Player_Name AS `HomeTeamBatsman`,
         PlayerScores.Bat_Runs As `Runs`,
		 PlayerScores.Bat_Balls As `Balls`, 
		 PlayerScores.HowOutId As `HowOut` 
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
         PlayerScores.Bat_Runs,
		 PlayerScores.Bat_Balls,
		 PlayerScores.HowOutId
     ORDER BY
         MAX(Bat_Runs) DESC 
         limit 3;

END //
delimiter;