DELIMITER //
﻿CREATE PROCEDURE usp_RecentMatches ()
BEGIN
		SELECT	homeTeam.Team_Name as `HomeTeam`,
				Teams.Team_Name as `OppponentTeam`,
				Result as `Summary`,
				case when Teams.FileName is null then "noLogo.png" else Teams.FileName End as `OpponentTeamLogo`,
				case when homeTeam.FileName is null then "noLogo.png" else homeTeam.FileName End as `HomeTeamTeamLogo`,
				Matches.MatchId ,
				Teams.TeamId as `OppTeamId`,
				homeTeam.TeamId as `HomeTeamId`,
				Matches.MatchOvers as `MatchOvers`,
				Matches.DateOfMatch,
				MT.MatchTypeName as `Type`,
				T.TournamentName as `Tournament`,
				u.UserName,
				(
					select TotalScore from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = OppponentTeamId			
				) as `OpponentsTeamScore`,
				(
					select TotalScore from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as `HomeTeamScore`,
				(
					select OppTeamOvers from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = OppponentTeamId			
				) as `OpponentsTeamOvers`,
				(
					select HomeTeamOvers from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as `HomeTeamOvers`,
				 (
					select wickets from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = HomeTeamId			
				) as `HomeTeamWickets`,
				(
					select wickets from TeamScores
					WHERE  TeamScores.MatchId = Matches.MatchId	and TeamScores.TeamId = Matches.OppponentTeamId					
				) as `OppenentTeamWickets`
		FROM Matches		
		inner join Teams on  Teams.TeamId = Matches.OppponentTeamId
		inner join MatchType MT on MT.MatchTypeId = Matches.MatchTypeId
		left join Tournaments T on T.TournamentId = Matches.TournamentId
		inner join Teams homeTeam on  homeTeam.TeamId = Matches.HomeTeamId
		inner join AspNetUsers u on Id = Matches.UserId
		order by Matches.MatchId Desc
		limit 3;	
END //
DELIMITER ;
