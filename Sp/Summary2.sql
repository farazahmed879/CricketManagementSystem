Alter procedure [usp_Summary2]
@paramHomeTeamId AS INT,
@paramOpponentTeamId AS INT,
@paramMatchId AS INT
AS
begin
	SELECT *
	FROM
		(
		SELECT	top 1
				homeTeam.Team_Name as 'HomeTeam',
				Teams.Team_Name as 'OppponentTeam',
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
					select TotalScore as 'HomeTeamScore'
					FROM TeamScores
					WHERE TeamId = @paramHomeTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as 'HomeTeamScore',
				 (
					select TotalScore as 'OpponentsTeamScore'
					FROM TeamScores
					WHERE TeamId = @paramOpponentTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as 'OpponentsTeamScore',
				 (
					select Wickets as 'HomeTeamWickets'
					FROM TeamScores
					WHERE TeamId = @paramHomeTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as 'HomeTeamWickets',
				 (
					select Wickets as 'OpponentTeamWickets'
					FROM TeamScores
					WHERE TeamId = @paramOpponentTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as 'OpponentTeamWickets'
				-- (
				--	select top 1 count (case when HomeTeamWickets.HowOutId != '7' then 1 else null end) over()  as 'HomeTeamWickets'
				--	FROM Players
				--	inner join PlayerScores HomeTeamWickets on Players.PlayerId = HomeTeamWickets.PlayerId
				--	inner join Matches on HomeTeamWickets.MatchId = Matches.MatchId
				--	WHERE  Players.TeamId = @paramHomeTeamId and 
				--	Matches.MatchId = @paramMatchId			
				--) as 'HomeTeamWickets',
				--(
				--	select top 1 count (case when TeamWickets.HowOutId != '7' then 1 else null end) over()  as 'OppenentTeamWickets'
				--	FROM Players
				--	inner join PlayerScores TeamWickets on Players.PlayerId = TeamWickets.PlayerId
				--	inner join Matches on TeamWickets.MatchId = Matches.MatchId
				--	WHERE  Players.TeamId = @paramOpponentTeamId and 
				--	Matches.MatchId = @paramMatchId
				--) as 'OppenentTeamWickets'
			
		
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

--exec [usp_Summary2] 3,5,1026
--select * from TeamScores where TeamId = 3 and MatchId = 1026

--select * from TeamScores