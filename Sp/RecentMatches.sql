Create procedure [usp_RecentMatches]
AS
begin
		SELECT	top 3
				homeTeam.Team_Name as 'HomeTeam',
				Teams.Team_Name as 'OppponentTeam',
				Result as 'Summary',
				Teams.[FileName] as 'OpponentTeamLogo',
				homeTeam.[FileName] as 'HomeTeamTeamLogo',
				--TeamScores.MatchId,
				Matches.MatchOvers as 'MatchOvers',
				--TeamScores.TeamId,
				Matches.DateOfMatch,
				MT.MatchTypeName as 'Type',
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
		--inner join TeamScores on TeamScores.TeamId = Teams.TeamId
		inner join MatchType MT on MT.MatchTypeId = Matches.MatchTypeId

		inner join Teams homeTeam on  homeTeam.TeamId = Matches.HomeTeamId
		--inner join TeamScores homeTeamScore on homeTeamScore.TeamId = homeTeam.TeamId
		inner join AspNetUsers u on Id = Matches.UserId
		order by Matches.MatchId Desc

end
go
