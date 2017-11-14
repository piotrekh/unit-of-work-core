using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb
{
    public interface IReleasesDataSets
    {
        DbSet<ReleaseEntity> Releases { get; }
    }
}
