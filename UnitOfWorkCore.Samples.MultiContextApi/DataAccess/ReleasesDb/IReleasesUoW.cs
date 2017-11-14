namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb
{
    /// <summary>
    /// This interface acts as a decorator for IUnitOfWork&lt;ReleasesDbContext&gt;,
    /// allowing to extend it with additional methods and properties
    /// </summary>
    public interface IReleasesUoW : IUnitOfWork<ReleasesDbContext>, IReleasesDataSets
    {
    }
}
