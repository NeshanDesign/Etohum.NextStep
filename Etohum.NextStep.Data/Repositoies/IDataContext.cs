using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Etohum.NextStep.Data.Model;

namespace Etohum.NextStep.Data.Repositoies

{
    public interface IDataContext : IDisposable 
    {
        void SaveAllChanges();
        DbSet<T> SetEntity<T>() where T : class;
        IEnumerable<DbEntityValidationResult> GetValidationErrorsEnumerable();
        DbEntityEntry<EntityBase<T>> SetEntry<T>(EntityBase<T> entity) where T : IComparable;

        DbSet<Subscriber> Subscribers { get;  }
        DbSet<SubscriptionHistory> SubscriptionHistores { get; }
    }

    public class SqlDbContext : DbContext, IDataContext
    {
        private bool _disposed;

        public SqlDbContext():base("etohumConnectionString")
        {
            Database.CreateIfNotExists();
            if (!Database.CompatibleWithModel(true))
            {
                Database.Delete();
                Database.Create();
            }
        }

        public SqlDbContext(string connectionString): base(connectionString)
        {
            // use eager laoding in case you need childeren nodes
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            // this part is in development environment, use DbMigrtion to have a more decent & more secure way to work with db
            Database.CreateIfNotExists();
            if (!Database.CompatibleWithModel(true))
            {
                Database.Delete(connectionString);
                Database.Create();
            }
        }


        public SqlDbContext(DbConnection connection, bool contextLimitedConnection = true)
            : base(connection, contextLimitedConnection)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.Connection.ConnectionString = connection.ConnectionString;
            Database.CreateIfNotExists();
        }
        
        
        // ReSharper disable RedundantOverridenMember
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // new ConfigurationManager().Configure(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public void SaveAllChanges()
        {
            SaveChanges();
        }

        public DbSet<T> SetEntity<T>() where T : class
        {
            return Set<T>();

        }

        public IEnumerable<DbEntityValidationResult> GetValidationErrorsEnumerable()
        {
            // not using now but it will be usefull 
            return new List<DbEntityValidationResult>(0);
        }

        DbEntityEntry<EntityBase<T>> IDataContext.SetEntry<T>(EntityBase<T> entity)
        {
            return Entry(entity);
        }

        public DbSet<object> SetEntity(object target)
        {
            return Set(target.GetType()).Attach(target) as DbSet<object>;

        }

        void IDisposable.Dispose()
        {
            try { SaveAllChanges(); }
            catch { /* do nothing, don't create loop  */ }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        public DbSet<Subscriber> Subscribers { get; }
        public DbSet<SubscriptionHistory> SubscriptionHistores { get; }
    }
}
