Alter procedure [usp_HomeScreen]
AS
begin
select 
count (Tournaments.TournamentId) As 'Tournaments',
count (Players.PlayerId) As 'Players',
count (Teams.TeamId) As 'Teams',
count (Matches.MatchId) As 'Matches',
count (PlayerScoreId) As 'Records',
Matches.Result as 'Result'
FROM PlayerScores
	Full Outer join Players ON PlayerScores.PlayerId = Players.PlayerId
	Full Outer join Teams ON Players.TeamId = Teams.TeamId
	Full Outer join Matches ON PlayerScores.MatchId = Matches.MatchId
	Full Outer join Tournaments On Matches.TournamentId = Tournaments.TournamentId

	GROUP BY Matches.MatchId
end

execute [usp_HomeScreen]

select * from Matches