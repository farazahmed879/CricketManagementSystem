
using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricketApp.Data
{
    public class CricketContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public CricketContext(DbContextOptions<CricketContext> options)
            : base(options)
        { }
        //public static readonly LoggerFactory MyConsoleLoggerFactory
        //    = new LoggerFactory(new[] {
        //       new ConsoleLoggerProvider((category, level)
        //          => category == DbLoggerCategory.Database.Command.Name
        //         && level == LogLevel.Information, true) });

        public DbSet<CricketApp.Domain.Team> Teams { get; set; }
        public DbSet<CricketApp.Domain.Player> Players { get; set; }
        public DbSet<CricketApp.Domain.Match> Matches { get; set; }
        public DbSet<CricketApp.Domain.PlayerScore> PlayerScores { get; set; }
        public DbSet<CricketApp.Domain.Tournament> Tournaments { get; set; }   
        public DbSet<TeamScore> TeamScores { get; set; }
        public DbSet<FallOfWicket> FallOFWickets { get; set; }
        public DbSet<MatchType> MatchType { get; set; }
        public DbSet<IdentityUser<int>> User { get; set; }
        public DbSet<IdentityRole<int>> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<HowOut> HowOut { get; set; }
        public DbSet<BattingStyle> BattingStyle { get; set; }
        public DbSet<BowlingStyle> BowlingStyle { get; set; }
        public DbSet<PlayerRole> PlayerRole { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder
                // .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(
                 "Server =(localdb)\\MSSQLLocalDB; Database = CricketAppDB; Trusted_Connection = True; ");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(s => s.OppponentTeam)
                .WithMany(s => s.OpponentTeamMatches)
                .HasForeignKey(s => s.OppponentTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                entity.HasOne(s => s.HomeTeam)
                .WithMany(s => s.HomeTeamMatches)
                .HasForeignKey(s => s.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });


            //It might not have been needed
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasMany(i => i.HomeTeamMatches)
                .WithOne(i => i.HomeTeam)
                .HasForeignKey(i => i.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                entity.HasMany(i => i.OpponentTeamMatches)
                .WithOne(i => i.OppponentTeam)
                .HasForeignKey(i => i.OppponentTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(i => i.ClubUserId)
                    .IsUnique()
                    .HasFilter("ClubUserId is not null");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                //entity.HasOne(i => i.User)
                
            });


            //modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            //{
            //    entity.HasKey(i => new { i.LoginProvider, i.UserId, i.ProviderKey });
            //});

            //modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            //{
            //    entity.HasKey(i => new { i.UserId,i.RoleId });
            //});
        }

    }
}
