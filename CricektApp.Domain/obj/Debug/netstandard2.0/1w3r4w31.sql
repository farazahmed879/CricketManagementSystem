IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
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
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [MatchType] (
    [MatchTypeId] int NOT NULL IDENTITY,
    [MatchTypeName] nvarchar(max) NULL,
    CONSTRAINT [PK_MatchType] PRIMARY KEY ([MatchTypeId])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] int NOT NULL,
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
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
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

CREATE TABLE [Teams] (
    [TeamId] int NOT NULL IDENTITY,
    [City] nvarchar(max) NOT NULL,
    [ClubUserId] int NOT NULL,
    [Place] nvarchar(max) NULL,
    [TeamLogo] varbinary(max) NULL,
    [Team_Name] nvarchar(max) NOT NULL,
    [Zone] nvarchar(max) NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([TeamId]),
    CONSTRAINT [FK_Teams_AspNetUsers_ClubUserId] FOREIGN KEY ([ClubUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Tournaments] (
    [TournamentId] int NOT NULL IDENTITY,
    [Organizor] nvarchar(max) NULL,
    [StartingDate] datetime2 NOT NULL,
    [TenantUserId] int NOT NULL,
    [TournamentName] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Tournaments] PRIMARY KEY ([TournamentId]),
    CONSTRAINT [FK_Tournaments_AspNetUsers_TenantUserId] FOREIGN KEY ([TenantUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Players] (
    [PlayerId] int NOT NULL IDENTITY,
    [Address] nvarchar(max) NULL,
    [BattingStyle] nvarchar(max) NULL,
    [BowlingStyle] nvarchar(max) NULL,
    [CNIC] nvarchar(max) NULL,
    [Contact] nvarchar(max) NULL,
    [DOB] datetime2 NULL,
    [Gender] nvarchar(max) NOT NULL,
    [IsDeactivated] bit NOT NULL,
    [IsGuestPlayer] bit NOT NULL,
    [PlayerLogo] varbinary(max) NULL,
    [Player_Name] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [TeamId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([PlayerId]),
    CONSTRAINT [FK_Players_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Matches] (
    [MatchId] int NOT NULL IDENTITY,
    [DateOfMatch] datetime2 NULL,
    [GroundName] nvarchar(max) NOT NULL,
    [HomeTeamId] int NOT NULL,
    [MatchLogo] varbinary(max) NULL,
    [MatchOvers] int NOT NULL,
    [MatchTypeId] int NOT NULL,
    [OppponentTeamId] int NOT NULL,
    [Place] nvarchar(max) NOT NULL,
    [Result] nvarchar(max) NOT NULL,
    [Season] int NULL,
    [Status] nvarchar(max) NOT NULL,
    [TournamentId] int NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Matches] PRIMARY KEY ([MatchId]),
    CONSTRAINT [FK_Matches_Teams_HomeTeamId] FOREIGN KEY ([HomeTeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Matches_MatchType_MatchTypeId] FOREIGN KEY ([MatchTypeId]) REFERENCES [MatchType] ([MatchTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Matches_Teams_OppponentTeamId] FOREIGN KEY ([OppponentTeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Matches_Tournaments_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments] ([TournamentId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [FallOFWickets] (
    [FallOfWicketId] int NOT NULL IDENTITY,
    [Eight] int NOT NULL,
    [Fifth] int NOT NULL,
    [First] int NOT NULL,
    [Fourth] int NOT NULL,
    [MatchId] int NOT NULL,
    [Ninth] int NOT NULL,
    [Second] int NOT NULL,
    [Seventh] int NOT NULL,
    [Sixth] int NOT NULL,
    [TeamId] int NOT NULL,
    [Tenth] int NOT NULL,
    [Third] int NOT NULL,
    CONSTRAINT [PK_FallOFWickets] PRIMARY KEY ([FallOfWicketId]),
    CONSTRAINT [FK_FallOFWickets_Matches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [Matches] ([MatchId]) ON DELETE CASCADE,
    CONSTRAINT [FK_FallOFWickets_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([TeamId]) ON DELETE CASCADE
);

GO

CREATE TABLE [PlayerScores] (
    [PlayerScoreId] int NOT NULL IDENTITY,
    [Ball_Runs] int NULL,
    [Bat_Balls] int NULL,
    [Bat_Runs] int NULL,
    [Bowler] nvarchar(max) NULL,
    [Catches] int NULL,
    [Four] int NULL,
    [HowOut] nvarchar(max) NULL,
    [IsPlayedInning] bit NOT NULL,
    [Maiden] int NULL,
    [MatchId] int NOT NULL,
    [Overs] int NULL,
    [PlayerId] int NOT NULL,
    [Position] int NOT NULL,
    [RunOut] int NULL,
    [Six] int NULL,
    [Stump] int NULL,
    [Wickets] int NULL,
    CONSTRAINT [PK_PlayerScores] PRIMARY KEY ([PlayerScoreId]),
    CONSTRAINT [FK_PlayerScores_Matches_MatchId] FOREIGN KEY ([MatchId]) REFERENCES [Matches] ([MatchId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlayerScores_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE CASCADE
);

GO

CREATE TABLE [TeamScores] (
    [TeamScoreId] int NOT NULL IDENTITY,
    [Byes] int NOT NULL,
    [LegByes] int NOT NULL,
    [MatchId] int NOT NULL,
    [NoBalls] int NOT NULL,
    [TeamId] int NOT NULL,
    [TotalScore] int NOT NULL,
    [Wideballs] int NOT NULL,
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

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

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

CREATE INDEX [IX_Players_TeamId] ON [Players] ([TeamId]);

GO

CREATE INDEX [IX_PlayerScores_MatchId] ON [PlayerScores] ([MatchId]);

GO

CREATE INDEX [IX_PlayerScores_PlayerId] ON [PlayerScores] ([PlayerId]);

GO

CREATE INDEX [IX_Teams_ClubUserId] ON [Teams] ([ClubUserId]);

GO

CREATE INDEX [IX_TeamScores_MatchId] ON [TeamScores] ([MatchId]);

GO

CREATE INDEX [IX_TeamScores_TeamId] ON [TeamScores] ([TeamId]);

GO

CREATE INDEX [IX_Tournaments_TenantUserId] ON [Tournaments] ([TenantUserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180709122101_initial', N'2.0.2-rtm-10011');

GO

ALTER TABLE [Matches] DROP CONSTRAINT [FK_Matches_Tournaments_TournamentId];

GO

ALTER TABLE [Tournaments] DROP CONSTRAINT [FK_Tournaments_AspNetUsers_TenantUserId];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Matches') AND [c].[name] = N'Status');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Matches] DROP COLUMN [Status];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Tournaments') AND [c].[name] = N'TenantUserId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Tournaments] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Tournaments] ALTER COLUMN [TenantUserId] int NULL;

GO

DROP INDEX [IX_Matches_TournamentId] ON [Matches];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Matches') AND [c].[name] = N'TournamentId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Matches] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Matches] ALTER COLUMN [TournamentId] int NOT NULL;
CREATE INDEX [IX_Matches_TournamentId] ON [Matches] ([TournamentId]);

GO

ALTER TABLE [Matches] ADD CONSTRAINT [FK_Matches_Tournaments_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments] ([TournamentId]) ON DELETE CASCADE;

GO

ALTER TABLE [Tournaments] ADD CONSTRAINT [FK_Tournaments_AspNetUsers_TenantUserId] FOREIGN KEY ([TenantUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180709202612_initial1', N'2.0.2-rtm-10011');

GO

ALTER TABLE [Teams] ADD [IsRegistered] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180710180730_ISregistered', N'2.0.2-rtm-10011');

GO

ALTER TABLE [Teams] DROP CONSTRAINT [FK_Teams_AspNetUsers_ClubUserId];

GO

DROP INDEX [IX_Teams_ClubUserId] ON [Teams];

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Teams') AND [c].[name] = N'ClubUserId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Teams] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Teams] ALTER COLUMN [ClubUserId] int NULL;

GO

ALTER TABLE [AspNetUserRoles] ADD [RoleId1] int NULL;

GO

ALTER TABLE [AspNetUserRoles] ADD [UserId1] int NULL;

GO

ALTER TABLE [AspNetUserRoles] ADD [Discriminator] nvarchar(max) NOT NULL DEFAULT N'';

GO

CREATE UNIQUE INDEX [IX_Teams_ClubUserId] ON [Teams] ([ClubUserId]) WHERE ClubUserId is not null;

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId1] ON [AspNetUserRoles] ([RoleId1]);

GO

CREATE INDEX [IX_AspNetUserRoles_UserId1] ON [AspNetUserRoles] ([UserId1]);

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId1] FOREIGN KEY ([RoleId1]) REFERENCES [AspNetRoles] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId1] FOREIGN KEY ([UserId1]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Teams] ADD CONSTRAINT [FK_Teams_AspNetUsers_ClubUserId] FOREIGN KEY ([ClubUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180711144146_LoginRegister1', N'2.0.2-rtm-10011');

GO

ALTER TABLE [PlayerScores] DROP CONSTRAINT [FK_PlayerScores_Players_PlayerId];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AspNetUserRoles') AND [c].[name] = N'Discriminator');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AspNetUserRoles] DROP COLUMN [Discriminator];

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'PlayerScores') AND [c].[name] = N'PlayerId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [PlayerScores] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [PlayerScores] ALTER COLUMN [PlayerId] int NULL;

GO

ALTER TABLE [PlayerScores] ADD CONSTRAINT [FK_PlayerScores_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([PlayerId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180712130900_IsNullPLayerScore', N'2.0.2-rtm-10011');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180712132825_IsNullOREmpty', N'2.0.2-rtm-10011');

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'PlayerScores') AND [c].[name] = N'HowOut');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [PlayerScores] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [PlayerScores] DROP COLUMN [HowOut];

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Players') AND [c].[name] = N'BattingStyle');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Players] DROP COLUMN [BattingStyle];

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Players') AND [c].[name] = N'BowlingStyle');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Players] DROP COLUMN [BowlingStyle];

GO

EXEC sp_rename N'Players.Role', N'PlayerRoleId', N'COLUMN';

GO

ALTER TABLE [PlayerScores] ADD [HowOutId] int NULL;

GO

ALTER TABLE [Players] ADD [BattingStyleId] int NULL;

GO

ALTER TABLE [Players] ADD [BowlingStyleId] int NULL;

GO

ALTER TABLE [Players] ADD [PlayerRoleId1] int NULL;

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

CREATE TABLE [PlayerRole] (
    [PlayerRoleId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_PlayerRole] PRIMARY KEY ([PlayerRoleId])
);

GO

CREATE INDEX [IX_PlayerScores_HowOutId] ON [PlayerScores] ([HowOutId]);

GO

CREATE INDEX [IX_Players_BattingStyleId] ON [Players] ([BattingStyleId]);

GO

CREATE INDEX [IX_Players_BowlingStyleId] ON [Players] ([BowlingStyleId]);

GO

CREATE INDEX [IX_Players_PlayerRoleId1] ON [Players] ([PlayerRoleId1]);

GO

ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_BattingStyle_BattingStyleId] FOREIGN KEY ([BattingStyleId]) REFERENCES [BattingStyle] ([BattingStyleId]) ON DELETE NO ACTION;

GO

ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_BowlingStyle_BowlingStyleId] FOREIGN KEY ([BowlingStyleId]) REFERENCES [BowlingStyle] ([BowlingStyleId]) ON DELETE NO ACTION;

GO

ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_PlayerRole_PlayerRoleId1] FOREIGN KEY ([PlayerRoleId1]) REFERENCES [PlayerRole] ([PlayerRoleId]) ON DELETE NO ACTION;

GO

ALTER TABLE [PlayerScores] ADD CONSTRAINT [FK_PlayerScores_HowOut_HowOutId] FOREIGN KEY ([HowOutId]) REFERENCES [HowOut] ([HowOutId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180712183041_PLayerTable', N'2.0.2-rtm-10011');

GO

ALTER TABLE [Players] DROP CONSTRAINT [FK_Players_PlayerRole_PlayerRoleId1];

GO

DROP INDEX [IX_Players_PlayerRoleId1] ON [Players];

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Players') AND [c].[name] = N'PlayerRoleId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Players] DROP COLUMN [PlayerRoleId];

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Players') AND [c].[name] = N'PlayerRoleId1');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Players] DROP COLUMN [PlayerRoleId1];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180714165456_deletePLayerRole', N'2.0.2-rtm-10011');

GO

ALTER TABLE [Players] ADD [PlayerRoleId] int NULL;

GO

CREATE INDEX [IX_Players_PlayerRoleId] ON [Players] ([PlayerRoleId]);

GO

ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_PlayerRole_PlayerRoleId] FOREIGN KEY ([PlayerRoleId]) REFERENCES [PlayerRole] ([PlayerRoleId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180714165735_PlayerRole', N'2.0.2-rtm-10011');

GO

