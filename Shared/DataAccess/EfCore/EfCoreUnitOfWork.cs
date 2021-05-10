using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAccess.EfCore
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        public readonly DbContext _context;
        private bool _disposed;

        public EfCoreUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            //Determine at runtime which db provider is being used, i.e if it is testing, sql server does not support these methods.
            //If DB provider is changed, this method needs to be updated.
            //https://stackoverflow.com/a/51487154/14363750
            if (_context.Database.IsSqlServer())
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;

                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                    _context.Database.OpenConnection();

                _context.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_context.Database.IsSqlServer())
            {
                _context.ChangeTracker.DetectChanges();

                SaveChanges();
                _context.Database.CommitTransaction();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Rollback()
        {
            if (_context.Database.IsSqlServer())
            {
                _context.Database.CurrentTransaction?.Rollback();
            }
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return (TDbContext)_context;
        }
    }
}
