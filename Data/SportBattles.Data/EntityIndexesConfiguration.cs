namespace SportBattles.Data
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using SportBattles.Data.Common.Models;
    using SportBattles.Data.Models;

    internal static class EntityIndexesConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            // IDeletableEntity.IsDeleted index
            var deletableEntityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }

            modelBuilder.Entity<GameMatch>().HasKey(k => new { k.GameId, k.MatchId });
            modelBuilder.Entity<GameMatch>().HasOne(gm => gm.Match).WithMany(g => g.Games).HasForeignKey(gm => gm.MatchId);
            modelBuilder.Entity<GameMatch>().HasOne(gm => gm.Game).WithMany(m => m.Matches).HasForeignKey(gm => gm.GameId);

            modelBuilder.Entity<GameTennisMatch>().HasKey(k => new { k.GameId, k.TennisMatchId });
            modelBuilder.Entity<GameTennisMatch>().HasOne(gtm => gtm.TennisMatch).WithMany(g => g.Games).HasForeignKey(gtm => gtm.TennisMatchId);
            modelBuilder.Entity<GameTennisMatch>().HasOne(gtm => gtm.Game).WithMany(tm => tm.TennisMatches).HasForeignKey(gtm => gtm.GameId);

            modelBuilder.Entity<Team>(t => t.HasIndex(cn => new { cn.CountryId, cn.Name }).IsUnique());
            modelBuilder.Entity<TennisPlayer>(t => t.HasIndex(cn => new { cn.CountryId, cn.Name }).IsUnique());
        }
    }
}
