using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkCore.MultiContext;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb
{
    /// <summary>
    /// This class acts as a decorator for IUnitOfWork&lt;ReleasesDbContext&gt;,
    /// allowing to extend it with additional methods and properties
    /// </summary>
    public class ReleasesUoW : IReleasesUoW
    {
        public const string KEY = "Releases";

        private readonly IUnitOfWork _uow;

        public DbSet<ReleaseEntity> Releases => _uow.Set<ReleaseEntity>();

        public ReleasesUoW(IUnitOfWorkPool uowPool)
        {
            _uow = uowPool.Get(KEY);
        }

        public void CommitTransaction()
        {
            _uow.CommitTransaction();
        }

        public void ForceBeginTransaction()
        {
            _uow.ForceBeginTransaction();
        }

        public void RollbackTransaction()
        {
            _uow.RollbackTransaction();
        }

        public int SaveChanges()
        {
            return _uow.SaveChanges();
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _uow.Set<T>();
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _uow.SetIsolationLevel(isolationLevel);
        }
    }
}
