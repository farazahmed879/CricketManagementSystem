
delimiter //
Create procedure usp_Summary2
(
	IN paramHomeTeamId INT,
	IN paramOpponentTeamId INT,
	IN paramMatchId INT
)
begin
	SELECT   homeTeam.Team_Name as `HomeTeam`,
				Teams.Team_Name as `OppponentTeam`,
				Matches.Result as `Result`,
				case when Teams.FileName is null then "noLogo.png" else  Teams.FileName end as `OpponentTeamLogo`,
				case when homeTeam.FileName is null then "noLogo.png" else  homeTeam.FileName end as `HomeTeamTeamLogo`,
				Ground.Name as `GroundName`,
				MatchType.MatchTypeName as `Type`,
				TournamentStages.Name as `Stage`,
				Players.Player_Name as `ManOfTheMatch`,
				Tournaments.TournamentName as `TournamentName`,
				Matches.DateOfMatch as `DateOfMatch`,
				Matches.HomeTeamOvers as `HomeTeamOvers`,
				Matches.OppTeamOvers as `OppTeamOvers`,
				 (
					select TeamScores.TotalScore as `HomeTeamScore`
					FROM TeamScores
					WHERE TeamScores.TeamId = paramHomeTeamId and TeamScores.MatchId = paramMatchId
						
				) as `HomeTeamScore`,
				 (
					select TeamScores.TotalScore as `OpponentsTeamScore`
					FROM TeamScores
					WHERE TeamScores.TeamId = paramOpponentTeamId and TeamScores.MatchId = paramMatchId
					
				) as `OpponentsTeamScore`,
				 (
					select TeamScores.Wickets as `HomeTeamWickets`
					FROM TeamScores
					WHERE TeamScores.TeamId = paramHomeTeamId and TeamScores.MatchId = paramMatchId			
						
				) as `HomeTeamWickets`,
				 (
					select TeamScores.Wickets as `OpponentTeamWickets`
					FROM TeamScores
					WHERE TeamScores.TeamId = paramOpponentTeamId and TeamScores.MatchId = paramMatchId 
				) as `OpponentTeamWickets`
	FROM Matches	
	inner join Teams on  Teams.TeamId = Matches.OppponentTeamId
	left join Players on  Players.PlayerId = Matches.PlayerOTM
	inner join Teams homeTeam on  homeTeam.TeamId = Matches.HomeTeamId
	left join TournamentStages on  TournamentStages.TournamentStageId = Matches.TournamentStageId
	inner join MatchType on  MatchType.MatchTypeId = Matches.MatchTypeId
	left join Tournaments on  Tournaments.TournamentId = Matches.TournamentId
	left join TeamScores homeTeamScore on homeTeamScore.TeamId = homeTeam.TeamId		
	left join TeamScores on TeamScores.TeamId = Teams.TeamId
	left join Ground on Ground.GroundId = Matches.GroundId
	where Matches.MatchId = paramMatchId	
	order by Matches.MatchId asc
	limit 1;
end //
delimiter;
