using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb
{
    public interface IIssuesDataSets
    {
        DbSet<IssueEntity> Issues { get; }
    }
}
