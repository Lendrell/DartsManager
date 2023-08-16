using MyDartsManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;
using System.Xml;

namespace MyDartsManager
{
    public class DartsDbContext : DbContext
    {
        public DbSet<ThrowCombination> ThrowCombinations { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }

        public DbSet<MatchStatistic> MatchStatistics { get; set; }

        public DbSet<PracticeTarget> PracticeTargets { get; set; }

        public DbSet<TrainingSession> TrainingSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies()
                              .UseSqlite("Data Source=darts.db");
            }

        }

        public void InitializeDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Match entity
            modelBuilder.Entity<Match>()
                .HasKey(m => m.MatchId);

            // Configure the Player entity
            modelBuilder.Entity<Player>()
                .HasKey(p => p.PlayerId);

            // Configure the Round entity
            modelBuilder.Entity<Round>()
                .HasKey(r => r.RoundId);

            modelBuilder.Entity<Round>()
                .HasOne(r => r.Match)
                .WithMany(m => m.Rounds)
                .HasForeignKey(r => r.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Round>()
                .HasOne(r => r.Player)
                .WithMany(p => p.Rounds)
                .HasForeignKey(r => r.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the ThrowCombination entity
            modelBuilder.Entity<ThrowCombination>()
                .HasKey(tc => tc.CombinationId);

            // Configure the MatchStatistics entity
            modelBuilder.Entity<MatchStatistic>()
                .HasKey(ms => ms.ID);

            modelBuilder.Entity<MatchStatistic>()
                .HasOne(ms => ms.Match)
                .WithMany(m => m.MatchStatistics)
                .HasForeignKey(ms => ms.MatchID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchStatistic>()
                .HasOne(ms => ms.Player)
                .WithMany(p => p.MatchStatistics)
                .HasForeignKey(ms => ms.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the TrainingSession entity
            modelBuilder.Entity<TrainingSession>()
                .HasKey(ts => ts.Id);

            modelBuilder.Entity<TrainingSession>()
                .HasOne(ts => ts.Player)
                .WithMany(p => p.TrainingSessions)
                .HasForeignKey(ts => ts.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrainingSession>()
            .Property(e => e.TrainingType)
            .HasConversion<string>();

            // Configure the SingleTargetPractice entity
            modelBuilder.Entity<PracticeTarget>()
                .HasKey(stp => stp.Id);

            modelBuilder.Entity<PracticeTarget>()
                .HasOne(stp => stp.TrainingSession)
                .WithMany(ts => ts.PracticeTargets)
                .HasForeignKey(stp => stp.TrainingSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
