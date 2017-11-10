using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkCore.Samples.SingleContextApi.DataAccess
{
    public static class UnitOfWorkExtensions
    {
        public static DbSet<ReleaseEntity> Releases(this IUnitOfWork uow)
        {
            return uow.Set<ReleaseEntity>();
        }
    }
}
