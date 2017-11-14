using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkCore.MultiContext;

namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb
{
    /// <summary>
    /// This class acts as a decorator for IUnitOfWork&lt;IssuesDbContext&gt;,
    /// allowing to extend it with additional methods and properties
    /// </summary>
    public class IssuesUow : IIssuesUoW
    {
        public const string KEY = "Issues";

        private readonly IUnitOfWork _uow;

        public DbSet<IssueEntity> Issues => _uow.Set<IssueEntity>();

        public IssuesUow(IUnitOfWorkPool uowPool)
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
