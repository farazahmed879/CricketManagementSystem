delimiter //
Create procedure usp_TotalAchieve()
begin
	SELECT *
	FROM
	(
		SELECT COUNT(1) as `Tournaments`
		FROM Tournaments
	) AS `Tournaments`,
	(
		SELECT	COUNT(1) as `Players`
		FROM Players
	) AS `players`,
		(
		SELECT	COUNT(1) as `Teams`
		FROM Teams
	) AS `Teams`,
		(
		SELECT	COUNT(1) as `Records`
		FROM PlayerScores
	) AS `PlayerScores`,
		(
		SELECT	COUNT(1) as `Matches`
		FROM Matches
	) AS `Matches`,
	(
		SELECT	COUNT(1) as `Series`
		FROM MatchSeries
	) AS `Series`;
end //
delimiter ;

