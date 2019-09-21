WITH Home AS(
    SELECT P.Player_Name AS Bowler,
           PS.Wickets As Wickets,
		   PS.Ball_runs as Runs,
           RANK() OVER (ORDER BY PS.Wickets desc) AS WicketsRank
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = 1
    AND M.MatchId = 1025),
Opponent AS (
    SELECT P.Player_Name AS Bowler,
           PS.Wickets As Wickets,
		   PS.Ball_runs as Runs,
           RANK() OVER (ORDER BY PS.Wickets desc) AS WicketsRank
    FROM PlayerScores PS
         JOIN Players P ON PS.PlayerId = P.PlayerId
         JOIN Teams T ON P.TeamId = T.TeamId
         JOIN Matches M  ON PS.MatchId = M.MatchId
    WHERE T.TeamId = 3
      AND M.MatchId = 1025
    )
SELECT H.Bowler AS HomeTeamBowler,
       H.Wickets AS HomeTeamWickets,
	   H.Runs AS HomeTeamWickets,
       A.Bowler AS OpponentTeamBowler,
       A.Wickets AS OpponentTeamWickets,
	   A.Runs AS OpponentTeamRuns
FROM Home H
     JOIN Opponent A ON H.WicketsRank = A.WicketsRank
WHERE H.WicketsRank <= 3;


