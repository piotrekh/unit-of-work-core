using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.SingleContextApi.DataAccess
{
    public class ReleasesDbContext : DbContext
    {
        public DbSet<ReleaseEntity> Releases { get; set; }

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
