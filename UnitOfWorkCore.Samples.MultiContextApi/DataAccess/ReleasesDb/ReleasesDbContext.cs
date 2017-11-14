using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb
{
    public class ReleasesDbContext : DbContext, IReleasesDataSets
    {
        public DbSet<ReleaseEntity> Releases => Set<ReleaseEntity>();

        public ReleasesDbContext(DbContextOptions<ReleasesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var releaseBuilder = builder.Entity<ReleaseEntity>();
            releaseBuilder.ToTable("Release", "dbo");
            releaseBuilder.HasKey(x => x.Id);
            releaseBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
