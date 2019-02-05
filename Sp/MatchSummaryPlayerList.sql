Alter PROCEDURE [usp_MatchSummaryPlayerList]
@paramHomeTeamId AS INT,
@paramOpponentTeamId AS INT,
@paramMatchId AS INT
AS
BEGIN 
WITH HomeTeam AS(
SELECT Batsman,
		Runs,
		Bowler,
		Wickets,
		BowlingRuns,
		RunsRank
FROM 
(
	    SELECT P.Player_Name AS Batsman,
           PS.Bat_Runs As Runs,
		   PS.TeamId,
           RANK() OVER (ORDER BY PS.Bat_Runs desc) AS RunsRank
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = @paramHomeTeamId
    AND M.MatchId = @paramMatchId
) HomeTeamBatting join 
(
	 SELECT P.Player_Name AS Bowler,
           PS.Wickets As Wickets,
		   PS.Ball_runs as BowlingRuns,
           RANK() OVER (ORDER BY PS.Wickets desc) AS WicketsRank,
		   ps.TeamId
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = @paramHomeTeamId
    AND M.MatchId = @paramMatchId
) HomeTeamBowling on HomeTeamBatting.TeamId = HomeTeamBowling.TeamId AND HomeTeamBatting.RunsRank = HomeTeamBowling.WicketsRank),

OpponentTeam as(
select 
		 OppoTeamBatsman,
		 OTBatsmanRuns,
		 OpponentTeamBowler,
		 OTBowlerWicket,
		 OTBowlerRuns,
		 RunsRank
from(
SELECT P.Player_Name AS OppoTeamBatsman,
           PS.Bat_Runs As OTBatsmanRuns,
		   PS.TeamId,
           RANK() OVER (ORDER BY PS.Bat_Runs desc) AS RunsRank
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = @paramOpponentTeamId
      AND M.MatchId = @paramMatchId
) OpponentTeamBatting join
(
SELECT P.Player_Name AS OpponentTeamBowler,
           PS.Wickets As OTBowlerWicket,
		   PS.Ball_runs as OTBowlerRuns,
           RANK() OVER (ORDER BY PS.Wickets desc) AS WicketsRank,
		   ps.TeamId
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = @paramOpponentTeamId
      AND M.MatchId = @paramMatchId
    
) OpponentTeamBowling on OpponentTeamBatting.TeamId = OpponentTeamBowling.TeamId AND OpponentTeamBatting.RunsRank = OpponentTeamBowling.WicketsRank)
SELECT HomeTeam.Batsman AS HomeBatsman,
       HomeTeam.Runs AS HomeBatsmanRuns,
       OppTeam.OppoTeamBatsman AS OppBatsman,
       OppTeam.OTBatsmanRuns AS OppBatsmanRuns,
	   HomeTeam.Bowler AS HomeBowler,
	   HomeTeam.Wickets AS HomeBowlerWicket,
	   HomeTeam.BowlingRuns AS HomeBowlerRuns,
	   OppTeam.OpponentTeamBowler AS OppBowler,
	   OppTeam.OTBowlerWicket AS OppBowlerWicket,
	   OppTeam.OTBowlerRuns AS OppBowlerRuns

FROM HomeTeam
     JOIN OpponentTeam OppTeam ON HomeTeam.RunsRank = OppTeam.RunsRank
WHERE HomeTeam.RunsRank <= 3;
	

END
go


exec  [usp_MatchSummaryPlayerList]