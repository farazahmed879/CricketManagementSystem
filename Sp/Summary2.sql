﻿delimiter //
create procedure usp_Summary2(@paramHomeTeamId AS INT,@paramOpponentTeamId AS INT,@paramMatchId AS INT)
begin
	SELECT *
	FROM
		(
		SELECT	top 1
				homeTeam.Team_Name as `HomeTeam`,
				Teams.Team_Name as `OppponentTeam`,
				Result as `Result`,
				case when Teams.[FileName] is null then `noLogo.png` else  Teams.[FileName] end as `OpponentTeamLogo`,
				case when homeTeam.[FileName] is null then `noLogo.png` else  homeTeam.[FileName] end as `HomeTeamTeamLogo`,
				--homeTeam.[FileName] as `HomeTeamTeamLogo`,
				Matches.GroundName as `GroundName`,
				MatchType.MatchTypeName as `Type`,
				TournamentStages.Name as `Stage`,
				Players.Player_Name as `ManOfTheMatch`,
				Matches.Place as `Place`,
				Tournaments.TournamentName as `TournamentName`,
				Matches.DateOfMatch as `DateOfMatch`,
				Matches.HomeTeamOvers as `HomeTeamOvers`,
				Matches.OppTeamOvers as `OppTeamOvers`,
				 (
					select TotalScore as `HomeTeamScore`
					FROM TeamScores
					WHERE TeamId = @paramHomeTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as `HomeTeamScore`,
				 (
					select TotalScore as `OpponentsTeamScore`
					FROM TeamScores
					WHERE TeamId = @paramOpponentTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as `OpponentsTeamScore`,
				 (
					select Wickets as `HomeTeamWickets`
					FROM TeamScores
					WHERE TeamId = @paramHomeTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
				) as `HomeTeamWickets`,
				 (
					select Wickets as `OpponentTeamWickets`
					FROM TeamScores
					WHERE TeamId = @paramOpponentTeamId and MatchId = @paramMatchId
					---make relation teamscore and matches	 
						
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

		where Matches.MatchId = @paramMatchId	
	
	order by Matches.MatchId 
	) AS `LastMatch`
end //
delimiter ;
