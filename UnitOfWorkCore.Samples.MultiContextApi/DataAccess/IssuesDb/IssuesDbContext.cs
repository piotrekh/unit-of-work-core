using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb
{
    public class IssuesDbContext : DbContext, IIssuesDataSets
    {
        public DbSet<IssueEntity> Issues => Set<IssueEntity>();

        public IssuesDbContext(DbContextOptions<IssuesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var releaseBuilder = builder.Entity<IssueEntity>();
            releaseBuilder.ToTable("Issue", "dbo");
            releaseBuilder.HasKey(x => x.Id);
            releaseBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
