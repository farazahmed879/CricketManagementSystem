
using CricektApp.Domain;
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
    public class CricketContext : IdentityDbContext<Login, IdentityRole<int>, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
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
        public DbSet<CricketApp.Domain.Login> Login { get; set; }
        public DbSet<TeamScore> TeamScores { get; set; }

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
