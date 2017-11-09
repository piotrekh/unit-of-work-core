using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;

namespace UnitOfWorkCore
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DbContext
    {
        protected readonly DbContext Context;
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;
        
        public UnitOfWork(T dbContext)
        {
            Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private void StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                if (_isolationLevel.HasValue)
                    _transaction = Context.Database.BeginTransaction(_isolationLevel.GetValueOrDefault());
                else
                    _transaction = Context.Database.BeginTransaction();
            }
        }

        public DbSet<Y> Set<Y>() where Y : class
        {
            return Context.Set<Y>();
        }

        public void ForceBeginTransaction()
        {
            StartNewTransactionIfNeeded();
        }

        public void CommitTransaction()
        {
            //do not open transaction here, because if during the request
            //nothing was changed (only select queries were run), we don't
            //want to open and commit an empty transaction - calling SaveChanges()
            //on _transactionProvider will not send any sql to database in such case
            Context.SaveChanges();

            if (_transaction != null)
            {
                _transaction.Commit();

                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;
        }

        public int SaveChanges()
        {
            StartNewTransactionIfNeeded();

            return Context.SaveChanges();
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            _transaction = null;
        }
    }
}
