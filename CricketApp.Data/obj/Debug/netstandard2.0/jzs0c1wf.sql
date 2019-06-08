IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BattingStyle] (
    [BattingStyleId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_BattingStyle] PRIMARY KEY ([BattingStyleId])
);

GO

CREATE TABLE [BowlingStyle] (
    [BowlingStyleId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_BowlingStyle] PRIMARY KEY ([BowlingStyleId])
);

GO

CREATE TABLE [HowOut] (
    [HowOutId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_HowOut] PRIMARY KEY ([HowOutId])
);

GO

CREATE TABLE [MatchType] (
    [MatchTypeId] int NOT NULL IDENTITY,
    [MatchTypeName] nvarchar(max) NULL,
    CONSTRAINT [PK_MatchType] PRIMARY KEY ([MatchTypeId])
);

GO

CREATE TABLE [PlayerRole] (
    [PlayerRoleId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_PlayerRole] PRIMARY KEY ([PlayerRoleId])
);

GO

CREATE TABLE [Teams] (
    [TeamId] int NOT NULL IDENTITY,
    [Team_Name] nvarchar(max) NOT NULL,
    [Place] nvarchar(max) NULL,
    [Zone] nvarchar(max) NULL,
    [Contact] nvarchar(max) NULL,
    [IsRegistered] bit NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [TeamLogo] varbinary(max) NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([TeamId])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    [UserId1] int NULL,
    [RoleId1] int NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId1] FOREIGN KEY ([RoleId1]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Tournaments] (
    [TournamentId] int NOT NULL IDENTITY,
    [TournamentName] nvarchar(max) NOT NULL,
    [Organizor] nvarchar(max) NULL,
    [StartingDate] datetime2 NULL,
    [UserId] int NOT NULL,
    [TenantUserId] int NULL,
    CONSTRAINT [PK_Tournaments] PRIMARY KEY ([TournamentId]),
    CONSTRAINT [FK_Tournaments_AspNetUsers_TenantUserId] FOREIGN KEY ([TenantUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ClubAdmins] (
    [ClubAdminId] int NOT NULL IDENTITY,
    [TeamId] int NOT NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_ClubAdmins] PRIMARY KEY ([ClubAdminId]),
    CONSTRAINT [FK_ClubAdmins_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClubAdmins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Players] (
    [PlayerId] int NOT NULL IDENTITY,
    [Player_Name] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NULL,
    [Gender] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NULL,
    [CNIC] nvarchar(max) NULL,
    [BattingStyleId] int NULL,
    [BowlingStyleId] int NULL,
    [PlayerRoleId] int NULL,
    [DOB] datetime2 NULL,
    [IsGuestPlayer] bit NOT NULL,
    [IsDeactivated] bit NOT NULL,
    [TeamId] int NOT NULL,
    [PlayerLogo] varbinary(max) NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([PlayerId]),
    CONSTRAINT [FK_Players_BattingStyle_BattingStyleId] FOREIGN KEY ([BattingStyleId]) REFERENCES [BattingStyle] ([BattingStyleId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Players_BowlingStyle_BowlingStyleId] FOREIGN KEY ([BowlingStyleId]) REFERENCES [BowlingStyle] ([BowlingStyleId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Players_PlayerRole_PlayerRoleId] FOREIGN KEY ([PlayerRoleId]) REFERENCES [PlayerRole] ([PlayerRoleId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Players_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Matches] (
    [MatchId] int NOT NULL IDENTITY,
    [GroundName] nvarchar(max) NOT NULL,
    [Place] nvarchar(max) NOT NULL,
    [MatchOvers] int NOT NULL,
    [Result] nvarchar(max) NOT NULL,
    [Season] int NULL,
    [TournamentId] int NULL,
    [DateOfMatch] datetime2 NULL,
    [HomeTeamId] int NOT NULL,
    [OppponentTeamId] int NOT NULL,
    [MatchLogo] varbinary(max) NULL,
    [UserId] int NOT NULL,
    [MatchTypeId] int NOT NULL,
    CONSTRAINT [PK_Matches] PRIMARY KEY ([MatchId]),
    CONSTRAINT [FK_Matches_Teams_HomeTeamId] FOREIGN KEY ([HomeTeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Matches_MatchType_MatchTypeId] FOREIGN KEY ([MatchTypeId]) REFERENCES [MatchType] ([MatchTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Matches_Teams_OppponentTeamId] FOREIGN KEY ([OppponentTeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Matches_Tournaments_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments] ([TournamentId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [FallOFWickets] (
    [FallOfWicketId] int NOT NULL IDENTITY,
    [MatchId] int NOT NULL,
    [TeamId] int NOT NULL,
    [First] int NOT NULL,
    [Second] int NOT NULL,
    [Third] int NOT NULL,
    [Fourth] int NOT NULL,
    [Fifth] int NOT NULL,
    [Sixth] int NOT NULL,
    [Seventh] int NOT NULL,
    [Eight] int NOT NULL,
    [Ninth] int NOT NULL,
    [Tenth] int NOT NULL,
    CONSTRAINT [PK_FallOFWickets] PRIMARY KEY ([FallOfWicketId]),
    CONSTRAINT [FK_FallOFWickets_Matches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [Matches] ([MatchId]) ON DELETE CASCADE,
    CONSTRAINT [FK_FallOFWickets_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE TABLE [PlayerScores] (
    [PlayerScoreId] int NOT NULL IDENTITY,
    [PlayerId] int NULL,
    [Position] int NOT NULL,
    [MatchId] int NOT NULL,
    [Bowler] nvarchar(max) NULL,
    [Bat_Runs] int NULL,
    [Bat_Balls] int NULL,
    [HowOutId] int NULL,
    [Ball_Runs] int NULL,
    [Overs] int NULL,
    [Wickets] int NULL,
    [Stump] int NULL,
    [Catches] int NULL,
    [Maiden] int NULL,
    [RunOut] int NULL,
    [Four] int NULL,
    [Six] int NULL,
    [TeamId] int NOT NULL,
    [IsPlayedInning] bit NOT NULL,
    CONSTRAINT [PK_PlayerScores] PRIMARY KEY ([PlayerScoreId]),
    CONSTRAINT [FK_PlayerScores_HowOut_HowOutId] FOREIGN KEY ([HowOutId]) REFERENCES [HowOut] ([HowOutId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PlayerScores_Matches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [Matches] ([MatchId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlayerScores_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PlayerScores_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE TABLE [TeamScores] (
    [TeamScoreId] int NOT NULL IDENTITY,
    [TotalScore] int NOT NULL,
    [Wideballs] int NOT NULL,
    [NoBalls] int NOT NULL,
    [Byes] int NOT NULL,
    [LegByes] int NOT NULL,
    [TeamId] int NOT NULL,
    [MatchId] int NOT NULL,
    CONSTRAINT [PK_TeamScores] PRIMARY KEY ([TeamScoreId]),
    CONSTRAINT [FK_TeamScores_Matches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [Matches] ([MatchId]) ON DELETE CASCADE,
    CONSTRAINT [FK_TeamScores_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId1] ON [AspNetUserRoles] ([RoleId1]);

GO

CREATE INDEX [IX_AspNetUserRoles_UserId1] ON [AspNetUserRoles] ([UserId1]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_ClubAdmins_TeamId] ON [ClubAdmins] ([TeamId]);

GO

CREATE INDEX [IX_ClubAdmins_UserId] ON [ClubAdmins] ([UserId]);

GO

CREATE INDEX [IX_FallOFWickets_MatchId] ON [FallOFWickets] ([MatchId]);

GO

CREATE INDEX [IX_FallOFWickets_TeamId] ON [FallOFWickets] ([TeamId]);

GO

CREATE INDEX [IX_Matches_HomeTeamId] ON [Matches] ([HomeTeamId]);

GO

CREATE INDEX [IX_Matches_MatchTypeId] ON [Matches] ([MatchTypeId]);

GO

CREATE INDEX [IX_Matches_OppponentTeamId] ON [Matches] ([OppponentTeamId]);

GO

CREATE INDEX [IX_Matches_TournamentId] ON [Matches] ([TournamentId]);

GO

CREATE INDEX [IX_Players_BattingStyleId] ON [Players] ([BattingStyleId]);

GO

CREATE INDEX [IX_Players_BowlingStyleId] ON [Players] ([BowlingStyleId]);

GO

CREATE INDEX [IX_Players_PlayerRoleId] ON [Players] ([PlayerRoleId]);

GO

CREATE INDEX [IX_Players_TeamId] ON [Players] ([TeamId]);

GO

CREATE INDEX [IX_PlayerScores_HowOutId] ON [PlayerScores] ([HowOutId]);

GO

CREATE INDEX [IX_PlayerScores_MatchId] ON [PlayerScores] ([MatchId]);

GO

CREATE INDEX [IX_PlayerScores_PlayerId] ON [PlayerScores] ([PlayerId]);

GO

CREATE INDEX [IX_PlayerScores_TeamId] ON [PlayerScores] ([TeamId]);

GO

CREATE INDEX [IX_TeamScores_MatchId] ON [TeamScores] ([MatchId]);

GO

CREATE INDEX [IX_TeamScores_TeamId] ON [TeamScores] ([TeamId]);

GO

CREATE INDEX [IX_Tournaments_TenantUserId] ON [Tournaments] ([TenantUserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180919152800_initial', N'2.1.4-rtm-31024');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180921110748_db', N'2.1.4-rtm-31024');

GO

CREATE TABLE [PlayerPastRecord] (
    [PlayerPastRecordId] int NOT NULL IDENTITY,
    [TotalMatch] int NULL,
    [TotalInnings] int NULL,
    [TotalNotOut] int NULL,
    [GetBowled] int NULL,
    [GetHitWicket] int NULL,
    [GetLBW] int NULL,
    [GetCatch] int NULL,
    [GetStump] int NULL,
    [GetRunOut] int NULL,
    [TotalBatRuns] int NULL,
    [TotalBatBalls] int NULL,
    [TotalFours] int NULL,
    [TotalSixes] int NULL,
    [NumberOf50s] int NULL,
    [NumberOf100s] int NULL,
    [TotalOvers] int NULL,
    [TotalBallRuns] int NULL,
    [TotalWickets] int NULL,
    [TotalMaidens] int NULL,
    [FiveWickets] int NULL,
    [DoBowled] int NULL,
    [DoHitWicket] int NULL,
    [DoLBW] int NULL,
    [DoCatch] int NULL,
    [DoStump] int NULL,
    [OnFieldCatch] int NULL,
    [OnFieldStump] int NULL,
    [OnFieldRunOut] int NULL,
    [PlayerId] int NOT NULL,
    CONSTRAINT [PK_PlayerPastRecord] PRIMARY KEY ([PlayerPastRecordId]),
    CONSTRAINT [FK_PlayerPastRecord_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_PlayerPastRecord_PlayerId] ON [PlayerPastRecord] ([PlayerId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180928103157_PlayerPastRecord', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Matches] ADD [MatchSeriesId] int NULL;

GO

CREATE TABLE [MatchSeries] (
    [MatchSeriesId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Organizor] nvarchar(max) NULL,
    [StartingDate] datetime2 NULL,
    [UserId] int NOT NULL,
    [TenantUserId] int NULL,
    CONSTRAINT [PK_MatchSeries] PRIMARY KEY ([MatchSeriesId]),
    CONSTRAINT [FK_MatchSeries_AspNetUsers_TenantUserId] FOREIGN KEY ([TenantUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Matches_MatchSeriesId] ON [Matches] ([MatchSeriesId]);

GO

CREATE INDEX [IX_MatchSeries_TenantUserId] ON [MatchSeries] ([TenantUserId]);

GO

ALTER TABLE [Matches] ADD CONSTRAINT [FK_Matches_MatchSeries_MatchSeriesId] FOREIGN KEY ([MatchSeriesId]) REFERENCES [MatchSeries] ([MatchSeriesId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181009144301_reinitial', N'2.1.4-rtm-31024');

GO

CREATE TABLE [MatchSchedule] (
    [MatchScheduleId] bigint NOT NULL IDENTITY,
    [GroundName] nvarchar(max) NULL,
    [OpponentTeam] nvarchar(max) NULL,
    [MatchOvers] int NOT NULL,
    [Month] nvarchar(max) NULL,
    [Day] int NOT NULL,
    [Year] int NOT NULL,
    CONSTRAINT [PK_MatchSchedule] PRIMARY KEY ([MatchScheduleId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181019111523_schedule', N'2.1.4-rtm-31024');

GO

ALTER TABLE [MatchSchedule] ADD [TeamId] int NOT NULL DEFAULT 0;

GO

CREATE INDEX [IX_MatchSchedule_TeamId] ON [MatchSchedule] ([TeamId]);

GO

ALTER TABLE [MatchSchedule] ADD CONSTRAINT [FK_MatchSchedule_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181019111747_scheduleRealation', N'2.1.4-rtm-31024');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MatchSchedule]') AND [c].[name] = N'MatchOvers');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [MatchSchedule] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [MatchSchedule] DROP COLUMN [MatchOvers];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MatchSchedule]') AND [c].[name] = N'Year');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MatchSchedule] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [MatchSchedule] ALTER COLUMN [Year] int NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MatchSchedule]') AND [c].[name] = N'Day');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [MatchSchedule] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [MatchSchedule] ALTER COLUMN [Day] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181022100036_mS', N'2.1.4-rtm-31024');

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Matches]') AND [c].[name] = N'Place');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Matches] ALTER COLUMN [Place] nvarchar(max) NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Matches]') AND [c].[name] = N'GroundName');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Matches] ALTER COLUMN [GroundName] nvarchar(max) NULL;

GO

ALTER TABLE [Matches] ADD [PlayerOTM] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181106120320_PlayerOfTheMatch', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Matches] ADD [PlayerId] int NULL;

GO

CREATE INDEX [IX_Matches_PlayerId] ON [Matches] ([PlayerId]);

GO

ALTER TABLE [Matches] ADD CONSTRAINT [FK_Matches_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181106151816_POTM', N'2.1.4-rtm-31024');

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'IsGuestPlayer');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Players] DROP COLUMN [IsGuestPlayer];

GO

ALTER TABLE [Players] ADD [IsGuestorRegistered] nvarchar(max) NULL;

GO

ALTER TABLE [Matches] ADD [MatchDescription] nvarchar(max) NULL;

GO

ALTER TABLE [Matches] ADD [TossWinningTeam] int NULL;

GO

CREATE TABLE [TournamentStages] (
    [TournamentStageId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_TournamentStages] PRIMARY KEY ([TournamentStageId])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181203151818_stages', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Matches] ADD [TournamentStageId] int NULL;

GO

CREATE INDEX [IX_Matches_TournamentStageId] ON [Matches] ([TournamentStageId]);

GO

ALTER TABLE [Matches] ADD CONSTRAINT [FK_Matches_TournamentStages_TournamentStageId] FOREIGN KEY ([TournamentStageId]) REFERENCES [TournamentStages] ([TournamentStageId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181203151900_stagesT', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Matches] ADD [HomeTeamOvers] real NOT NULL DEFAULT CAST(0 AS real);

GO

ALTER TABLE [Matches] ADD [OppTeamOvers] real NOT NULL DEFAULT CAST(0 AS real);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181205145756_HOMEOPPOvers', N'2.1.4-rtm-31024');

GO

ALTER TABLE [PlayerPastRecord] ADD [BestScore] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181206131822_BestScore', N'2.1.4-rtm-31024');

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Matches]') AND [c].[name] = N'OppTeamOvers');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Matches] ALTER COLUMN [OppTeamOvers] real NULL;

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Matches]') AND [c].[name] = N'HomeTeamOvers');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Matches] ALTER COLUMN [HomeTeamOvers] real NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181219114450_playedOvers', N'2.1.4-rtm-31024');

GO

ALTER TABLE [TeamScores] ADD [Wickets] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190103072036_TeamScoreWickets', N'2.1.4-rtm-31024');

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlayerScores]') AND [c].[name] = N'Overs');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [PlayerScores] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [PlayerScores] ALTER COLUMN [Overs] real NULL;

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlayerPastRecord]') AND [c].[name] = N'TotalOvers');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [PlayerPastRecord] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [PlayerPastRecord] ALTER COLUMN [TotalOvers] real NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190117131852_PlayerScoreOversIntToFoat', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Players] ADD [FileName] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190325202558_playerFileName', N'2.1.4-rtm-31024');

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'PlayerLogo');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Players] DROP COLUMN [PlayerLogo];

GO

ALTER TABLE [Teams] ADD [FileName] nvarchar(max) NULL;

GO

ALTER TABLE [Matches] ADD [FileName] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190403180246_FileName', N'2.1.4-rtm-31024');

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Teams]') AND [c].[name] = N'TeamLogo');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Teams] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Teams] DROP COLUMN [TeamLogo];

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Matches]') AND [c].[name] = N'MatchLogo');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Matches] DROP COLUMN [MatchLogo];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190403190100_RemoveImageBytesCodes', N'2.1.4-rtm-31024');

GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlayerScores]') AND [c].[name] = N'Bowler');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [PlayerScores] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [PlayerScores] DROP COLUMN [Bowler];

GO

ALTER TABLE [PlayerScores] ADD [BowlerId] int NULL;

GO

CREATE INDEX [IX_PlayerScores_BowlerId] ON [PlayerScores] ([BowlerId]);

GO

ALTER TABLE [PlayerScores] ADD CONSTRAINT [FK_PlayerScores_Players_BowlerId] FOREIGN KEY ([BowlerId]) REFERENCES [Players] ([PlayerId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190415162313_bowler2', N'2.1.4-rtm-31024');

GO

ALTER TABLE [HowOut] ADD [Normalize] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190415204710_howOutNomalize', N'2.1.4-rtm-31024');

GO

ALTER TABLE [PlayerScores] ADD [Fielder] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190506150017_addFielderField', N'2.1.4-rtm-31024');

GO

ALTER TABLE [Tournaments] ADD [FileName] nvarchar(max) NULL;

GO

ALTER TABLE [MatchSeries] ADD [FileName] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190602110540_TournamentImage', N'2.1.4-rtm-31024');

GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Teams]') AND [c].[name] = N'Place');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Teams] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Teams] ALTER COLUMN [Place] nvarchar(100) NULL;

GO

ALTER TABLE [AspNetUsers] ADD [FirstName] nvarchar(max) NULL;

GO

ALTER TABLE [AspNetUsers] ADD [LastName] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190608150032_addfieldFirstNameLastName', N'2.1.4-rtm-31024');

GO

DROP TABLE [ClubAdmins];

GO

ALTER TABLE [Teams] ADD [TenantUserId] int NULL;

GO

ALTER TABLE [Teams] ADD [UserId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [AspNetUsers] ADD [ApplicationUserRoleId] int NULL;

GO

CREATE INDEX [IX_Teams_TenantUserId] ON [Teams] ([TenantUserId]);

GO

CREATE INDEX [IX_AspNetUsers_ApplicationUserRoleId] ON [AspNetUsers] ([ApplicationUserRoleId]);

GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_AspNetRoles_ApplicationUserRoleId] FOREIGN KEY ([ApplicationUserRoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Teams] ADD CONSTRAINT [FK_Teams_AspNetUsers_TenantUserId] FOREIGN KEY ([TenantUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190608173515_RemoveClubAdminAddRoles', N'2.1.4-rtm-31024');

GO
-----------------------
ALTER TABLE [Teams] DROP CONSTRAINT [FK_Teams_AspNetUsers_TenantUserId];

GO

DROP INDEX [IX_Teams_TenantUserId] ON [Teams];

GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Teams]') AND [c].[name] = N'TenantUserId');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Teams] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Teams] DROP COLUMN [TenantUserId];

GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Teams]') AND [c].[name] = N'UserId');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Teams] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Teams] ALTER COLUMN [UserId] int NULL;

GO

update Teams set userId = null
GO

CREATE UNIQUE INDEX [IX_Teams_UserId] ON [Teams] ([UserId]) WHERE [UserId] IS NOT NULL;

GO

ALTER TABLE [Teams] ADD CONSTRAINT [FK_Teams_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190608175128_addTeamReferenceInUSers', N'2.1.4-rtm-31024');

GO

