SELECT DISTINCT PlayerScores.Ball_Runs,w.Wickets,PlayerScores.PlayerId
FROM PlayerScores
INNER JOIN
(
	SELECT MAX(Wickets) AS 'Wickets',
		   ps.PlayerId
	FROM PlayerScores as ps
	GROUP BY ps.PlayerId
) AS w ON PlayerScores.PlayerId = w.PlayerId AND w.Wickets = PlayerScores.Wickets