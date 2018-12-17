Alter procedure [usp_Summary2]
@paramMatchId AS INT
AS
begin
	SELECT *
	FROM
		(
		SELECT	top 1
				homeTeam.Team_Name as 'HomeTeam',
				Teams.Team_Name as 'OppponentTeam',
				homeTeamScore.TotalScore as 'HomeTeamScore',
				TeamScores.TotalScore as 'OpponentsTeamScore',
				Result as 'Result',
				Teams.TeamLogo as 'OpponentTeamLogo',
				homeTeam.TeamLogo as 'HomeTeamTeamLogo',
				Matches.GroundName as 'GroundName',
				MatchType.MatchTypeName as 'Type',
				TournamentStages.Name as 'Stage',
				Players.Player_Name as 'ManOfTheMatch',
				Matches.Place as 'Place',
				Tournaments.TournamentName as 'TournamentName',
				Matches.DateOfMatch as 'DateOfMatch',
				Matches.HomeTeamOvers as 'HomeTeamOvers',
				Matches.OppTeamOvers as 'OppTeamOvers',
				 (
					select top 1 count (case when HomeTeamWickets.HowOutId != '7' then 1 else null end) over()  as 'HomeTeamWickets'
					FROM Players
					inner join PlayerScores HomeTeamWickets on Players.PlayerId = HomeTeamWickets.PlayerId
					WHERE  Players.TeamId = homeTeam.TeamId					
				) as 'HomeTeamWickets',
				(
					select top 1 count (case when TeamWickets.HowOutId != '7' then 1 else null end) over()  as 'HomeTeamWickets'
					FROM Players
					inner join PlayerScores TeamWickets on Players.PlayerId = TeamWickets.PlayerId
					WHERE  Players.TeamId = Teams.TeamId					
				) as 'OppenentTeamWickets'
			
		
		FROM Matches	
		inner join Teams on  Teams.TeamId = Matches.OppponentTeamId
		left join Players on  Players.PlayerId = Matches.PlayerOTM
		inner join TeamScores on TeamScores.TeamId = Teams.TeamId
		inner join Teams homeTeam on  homeTeam.TeamId = Matches.HomeTeamId
		inner join TeamScores homeTeamScore on homeTeamScore.TeamId = homeTeam.TeamId
		left join TournamentStages on  TournamentStages.TournamentStageId = Matches.TournamentStageId
		inner join MatchType on  MatchType.MatchTypeId = Matches.MatchTypeId
		left join Tournaments on  Tournaments.TournamentId = Matches.TournamentId
		
		where Matches.MatchId = @paramMatchId
	
	order by Matches.MatchId 
	) AS LastMatch
end
go

exec [usp_Summary2] 1025