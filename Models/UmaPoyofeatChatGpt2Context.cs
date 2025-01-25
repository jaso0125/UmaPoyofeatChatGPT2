using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UmaPoyofeatChatGPT2.Models
{
    public partial class UmaPoyofeatChatGpt2Context : DbContext
    {
        public UmaPoyofeatChatGpt2Context()
        {
        }

        public UmaPoyofeatChatGpt2Context(DbContextOptions<UmaPoyofeatChatGpt2Context> options) : base(options)
        {
        }

        public virtual DbSet<HorseRace> HorseRaces { get; set; }

        public virtual DbSet<PastRace> PastRaces { get; set; }

        public virtual DbSet<RaceInfo> RaceInfos { get; set; }

        public virtual DbSet<Result> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: new[] { 40613, 10928, 10929 } // 再試行するエラー番号
                        );
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HorseRace>(entity =>
            {
                entity.HasKey(e => e.HorseRaceInfoId).HasName("PK__HorseRac__EEEEA78753E28659");

                entity.ToTable("HorseRace");

                entity.HasIndex(e => new { e.RaceId, e.KettoNum }, "IX_RaceId_KettoNum").IsUnique();

                entity.Property(e => e.GenderAge).HasMaxLength(50);
                entity.Property(e => e.HorseName).HasMaxLength(100);
                entity.Property(e => e.Jockey).HasMaxLength(100);
                entity.Property(e => e.KettoNum).HasMaxLength(50);
                entity.Property(e => e.Kinryo).HasMaxLength(50);
                entity.Property(e => e.RaceId).HasMaxLength(50);
                entity.Property(e => e.TrainerComment).HasMaxLength(500);
                entity.Property(e => e.TrainingTime).HasMaxLength(200);
                entity.Property(e => e.Umaban).HasMaxLength(50);
                entity.Property(e => e.Wakuban).HasMaxLength(50);
                entity.Property(e => e.WeightChange)
                    .HasMaxLength(50)
                    .HasDefaultValue("");
                entity.Property(e => e.CreateDate);
                entity.Property(e => e.UpdateDate);
            });

            modelBuilder.Entity<PastRace>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__PastRace__3214EC072089B2AE");

                entity.ToTable("PastRace");

                entity.HasIndex(e => e.KettoNum, "IX_KettoNum");

                entity.Property(e => e.Date).HasMaxLength(50);
                entity.Property(e => e.Distance).HasMaxLength(50);
                entity.Property(e => e.FinishPosition).HasMaxLength(50);
                entity.Property(e => e.HorseName).HasMaxLength(50);
                entity.Property(e => e.HorseNumber).HasMaxLength(50);
                entity.Property(e => e.Jockey).HasMaxLength(100);
                entity.Property(e => e.KettoNum).HasMaxLength(50);
                entity.Property(e => e.Margin).HasMaxLength(50);
                entity.Property(e => e.NumberOfHorses).HasMaxLength(50);
                entity.Property(e => e.Odds).HasMaxLength(50);
                entity.Property(e => e.Popularity).HasMaxLength(50);
                entity.Property(e => e.PostPosition).HasMaxLength(50);
                entity.Property(e => e.RaceName).HasMaxLength(100);
                entity.Property(e => e.Racecourse).HasMaxLength(100);
                entity.Property(e => e.RunningOrder).HasMaxLength(50);
                entity.Property(e => e.StartTime).HasMaxLength(50);
                entity.Property(e => e.TrackCondition).HasMaxLength(50);
                entity.Property(e => e.TrainerComment).HasMaxLength(1000);
                entity.Property(e => e.Weather).HasMaxLength(50);
                entity.Property(e => e.Weight).HasMaxLength(50);
                entity.Property(e => e.WeightChange).HasMaxLength(50);
            });

            modelBuilder.Entity<RaceInfo>(entity =>
            {
                entity.HasKey(e => e.RaceId).HasName("PK__RaceInfo__05FBD6B4523D8AFD");

                entity.ToTable("RaceInfo");

                entity.HasIndex(e => e.RaceId, "IX_RaceInfo_RaceId");

                entity.Property(e => e.RaceId).HasMaxLength(50);
                entity.Property(e => e.Date).HasMaxLength(10);
                entity.Property(e => e.DirtTrackCondition).HasMaxLength(50);
                entity.Property(e => e.Distance).HasMaxLength(50);
                entity.Property(e => e.RaceCourse).HasMaxLength(50);
                entity.Property(e => e.RaceName).HasMaxLength(255);
                entity.Property(e => e.RaceNumber).HasMaxLength(50);
                entity.Property(e => e.ShibaTrackCondition).HasMaxLength(50);
                entity.Property(e => e.StartTime).HasMaxLength(10);
                entity.Property(e => e.Weather).HasMaxLength(50);
                entity.Property(e => e.IsWin5Race);
                entity.Property(e => e.CreateDate);
                entity.Property(e => e.UpdateDate);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => e.RaceResultId).HasName("PK__Result__A7D12EE2756A30DA");

                entity.ToTable("Result");

                entity.HasIndex(e => new { e.RaceId, e.Date }, "IDX_RaceId_Date");

                entity.Property(e => e.CornerPassOrder).HasMaxLength(50);
                entity.Property(e => e.Date).HasMaxLength(50);
                entity.Property(e => e.Distance).HasMaxLength(50);
                entity.Property(e => e.HorseName).HasMaxLength(100);
                entity.Property(e => e.JockeyName).HasMaxLength(100);
                entity.Property(e => e.JyoName).HasMaxLength(50);
                entity.Property(e => e.Popularity).HasMaxLength(50);
                entity.Property(e => e.RaceId).HasMaxLength(50);
                entity.Property(e => e.RaceName).HasMaxLength(100);
                entity.Property(e => e.RaceNum).HasMaxLength(3);
                entity.Property(e => e.Result1)
                    .HasMaxLength(3)
                    .HasColumnName("Result");
                entity.Property(e => e.Time).HasMaxLength(50);
                entity.Property(e => e.Umaban).HasMaxLength(50);
                entity.Property(e => e.Wakuban).HasMaxLength(50);
            });
        }
    }
}