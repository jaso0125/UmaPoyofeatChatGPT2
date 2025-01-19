using Microsoft.EntityFrameworkCore;
using UmaPoyofeatChatGPT2.Models;

namespace UmaPoyofeatChatGPT2.Data
{
    public class UmaPoyofeatChatGPT2Context : DbContext
    {
        public UmaPoyofeatChatGPT2Context(DbContextOptions<UmaPoyofeatChatGPT2Context> options) : base(options)
        {
        }

        public DbSet<HorseRace> HorseRaces { get; set; }
        public DbSet<PastRace> PastRaces { get; set; }
        public DbSet<RaceInfo> RaceInfos { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}