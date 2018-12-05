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
    WHERE T.TeamId = 1
    AND M.MatchId = 1025
) HomeTeamBatting join (
	 SELECT P.Player_Name AS Bowler,
           PS.Wickets As Wickets,
		   PS.Ball_runs as BowlingRuns,
           RANK() OVER (ORDER BY PS.Wickets desc) AS WicketsRank,
		   ps.TeamId
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = 1
    AND M.MatchId = 1025
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
    WHERE T.TeamId = 3
      AND M.MatchId = 1025
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
    WHERE T.TeamId = 3
      AND M.MatchId = 1025
    
) OpponentTeamBowling on OpponentTeamBatting.TeamId = OpponentTeamBowling.TeamId AND OpponentTeamBatting.RunsRank = OpponentTeamBowling.WicketsRank)
SELECT HomeTeam.Batsman AS HomeTeamBatsman,
       HomeTeam.Runs AS HomeTeamRuns,
       OppTeam.OppoTeamBatsman AS OpponentTeamBatsman,
       OppTeam.OTBatsmanRuns AS OpponentTeamRuns,
	   HomeTeam.Bowler AS HomeTeamBowler,
	   HomeTeam.Wickets AS HTBowlerWicket,
	   HomeTeam.BowlingRuns AS HTBowlerRuns,
	   OppTeam.OpponentTeamBowler AS OpponentTeamBowler,
	   OppTeam.OTBowlerWicket AS OTBowlerWicket,
	   OppTeam.OTBowlerRuns AS OTBowlerRuns

FROM HomeTeam
     JOIN OpponentTeam OppTeam ON HomeTeam.RunsRank = OppTeam.RunsRank
WHERE HomeTeam.RunsRank <= 3;