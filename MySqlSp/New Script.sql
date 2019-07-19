INSERT AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (16, N'Club Admin', N'CLUB ADMIN', NULL)
INSERT AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (19, N'Administrator', N'ADMINISTRATOR', NULL)

INSERT AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES (2, N'inqilab001', N'INQILAB001', N'mehmoodkhan84@gmail.com', N'MEHMOODKHAN84@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEMRbMUEPtDBwC2zY6pG1yARe/WL/DNiGNyOcwxW7w0nSK1iPXhtQip4qN1aLsVu0Iw==', N'83d6edf9-2bf7-4cd9-8c53-1a7f6f7c453b', N'966a7732-ddbf-4674-9197-7d02de5808d9', NULL, 0, 0, NULL, 1, 0)
INSERT AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES (3, N'hammad123', N'HAMMAD123', N'hammad@abc.com', N'HAMMAD@ABC.COM', 0, N'AQAAAAEAACcQAAAAEI7FTbDlDS7rMjLjkdCXq9YxF9RX3HhgyhGi/TyRXrrQeLNRNG0wlNG7HcmY1yGiMQ==', N'3CBAPAP3HUEXQOJVXYBPBSUSMTCNZR6Q', N'c91385ec-5a2d-4b85-8526-8ffccac36aba', NULL, 0, 0, NULL, 1, 0)

INSERT AspNetUserRoles (UserId, RoleId, UserId1, RoleId1) VALUES (1, 19, NULL, NULL)

INSERT BattingStyle (BattingStyleId, Name) VALUES (1, N'Left-hand');
INSERT BattingStyle (BattingStyleId, Name) VALUES (2, N'Right-hand');

INSERT BowlingStyle (BowlingStyleId, Name) VALUES (1, N'Right-arm fast')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (2, N'Left-arm fast')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (3, N'Left-arm medium')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (4, N'Right-arm medium')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (5, N'Right-arm medium fast')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (6, N'Left-arm medium fast')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (7, N'Leg break (right-arm)')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (8, N'Off-spin (right-arm)')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (9, N'Slow left-arm spin')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (10, N'Left-arm orthodox')
INSERT BowlingStyle (BowlingStyleId, Name) VALUES (11, N'Right arm (wrist spiner)')


INSERT HowOut (HowOutId, Name) VALUES (1, N'Catch')
INSERT HowOut (HowOutId, Name) VALUES (2, N'Bowled')
INSERT HowOut (HowOutId, Name) VALUES (3, N'Stumped')
INSERT HowOut (HowOutId, Name) VALUES (4, N'Run Out')
INSERT HowOut (HowOutId, Name) VALUES (5, N'LBW')
INSERT HowOut (HowOutId, Name) VALUES (6, N'Hit-wicket')
INSERT HowOut (HowOutId, Name) VALUES (7, N'Not Out')


INSERT MatchType (MatchTypeId, MatchTypeName) VALUES (1, N'Friendly')
INSERT MatchType (MatchTypeId, MatchTypeName) VALUES (2, N'Tournament')
INSERT MatchType (MatchTypeId, MatchTypeName) VALUES (3, N'Series')

SET IDENTITY_INSERT PlayerRole ON 



INSERT TournamentStages (TournamentStageId, `Name`) VALUES (1, 'Group Stage')
INSERT TournamentStages (TournamentStageId, `Name`) VALUES (2, 'League Stage')
INSERT TournamentStages (TournamentStageId, `Name`) VALUES (3, 'Pre Quater Finals')
INSERT TournamentStages (TournamentStageId, `Name`) VALUES (4, 'Quater Finals')
INSERT TournamentStages (TournamentStageId, `Name`) VALUES (5, 'Semi Finals')
INSERT TournamentStages (TournamentStageId, `Name`) VALUES (6, 'Finals')

INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (1, N'Gulzar Cricket Club', N'Malir City', N'4', N'03353928246', 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (2, N'Pak Shaheen c c', N'MALIR', N'4', N'0337-3167977', 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (3, N'Inqilab Cricket Club', N'QUAIDABAD', N'4', N'0313-2276376', 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (4, N'Tuba Sports c c', N'RAFY AHAM', N'4', NULL, 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (5, N'Union Sports c c ', N'MALIR HALT', N'4', NULL, 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (6, N'Hansabad GYM', N'MALIR', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (7, N'Orient Star C C', N'MALIR', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (8, N'Hansot GYM', N'SHAH FAISEL', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (9, N'Bright Cricket Club', N'PIA Colony', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (10, N'Young Pak Flag', N'Malir ', N'4', NULL, 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (11, N'Millat Cricket Club ', N'Shah Faisal ', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (12, N'Faisal Gym C C', N'Shah faisal', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (13, N'Saudabad Gym', N'MALIR', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (14, N'Sind Rana C C', N'Quaidabad', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (15, N'Quaidabad Gym', N'Quaidabad ', N'4', NULL, 1, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (16, N'Steel town Gym', N'STEEL TOWN', N'4', NULL, 0, N'Karachi')
INSERT Teams (TeamId, Team_Name, Place, Zone, Contact, IsRegistered, City) VALUES (17, N'Karachi Eglets c c', N'PIA Colony', N'4', NULL, 0, N'Karachi')


INSERT PlayerRole (PlayerRoleId, Name) VALUES (1, N'Batsman');
INSERT PlayerRole (PlayerRoleId, Name) VALUES (2, N'Bowler');
INSERT PlayerRole (PlayerRoleId, Name) VALUES (3, N'All-Rounder');
INSERT PlayerRole (PlayerRoleId, Name) VALUES (4, N'Wicket-Keeper Batsman');
