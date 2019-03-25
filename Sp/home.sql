﻿Alter procedure [usp_HomeScreen]
AS
begin
	SELECT *
	FROM
	(
		SELECT COUNT(1) as 'Tournaments'
		FROM Tournaments
	) AS Tournaments,
	(
		SELECT	COUNT(1) as 'Players'
		FROM Players
	) AS players,
		(
		SELECT	COUNT(1) as 'Teams'
		FROM Teams
	) AS Teams,
		(
		SELECT	COUNT(1) as 'Records'
		FROM PlayerScores
	) AS PlayerScores,
		(
		SELECT	COUNT(1) as 'Matches'
		FROM Matches
	) AS Matches,
	(
		SELECT	COUNT(1) as 'Series'
		FROM MatchSeries
	) AS Series,

		(
		SELECT	top 1
				homeTeam.Team_Name as 'HomeTeam',
				Teams.Team_Name as 'OppponentTeam',
				Result as 'Summary',
				Teams.TeamLogo as 'OpponentTeamLogo',
				homeTeam.TeamLogo as 'HomeTeamTeamLogo',
				TeamScores.MatchId,
				TeamScores.TeamId,
				Matches.DateOfMatch,
				u.UserName,
				(
					select TotalScore from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = OppponentTeamId			
				) as 'OpponentsTeamScore',
				(
					select TotalScore from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as 'HomeTeamScore',
				(
					select OppTeamOvers from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = OppponentTeamId			
				) as 'OpponentsTeamOvers',
				(
					select HomeTeamOvers from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as 'HomeTeamOvers',
				 (
					select wickets from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as 'HomeTeamWickets',
				(
					select wickets from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = Matches.OppponentTeamId					
				) as 'OppenentTeamWickets'
			
		
		FROM Matches		
		inner join Teams on  Teams.TeamId = Matches.OppponentTeamId
		inner join TeamScores on TeamScores.TeamId = Teams.TeamId

		inner join Teams homeTeam on  homeTeam.TeamId = Matches.HomeTeamId
		inner join TeamScores homeTeamScore on homeTeamScore.TeamId = homeTeam.TeamId
		inner join AspNetUsers u on Id = Matches.UserId
		order by Matches.MatchId Desc
	) AS LastMatch
end
go
