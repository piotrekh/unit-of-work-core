namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb
{
    /// <summary>
    /// This interface acts as a decorator for IUnitOfWork&lt;IssuesDbContext&gt;,
    /// allowing to extend it with additional methods and properties
    /// </summary>
    public interface IIssuesUoW : IUnitOfWork<IssuesDbContext>, IIssuesDataSets
    {
    }
}
