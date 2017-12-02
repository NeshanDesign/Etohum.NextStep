using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Etohum.NextStep.Data.Model;

namespace Etohum.NextStep.Data.Repositoies
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>, IDisposable
        where TEntity : EntityBase<TKey> where TKey : IComparable
    {
        protected IDataContext Context;

       
        protected RepositoryBase()
        {
            Context = new SqlDbContext("etohumConnectionString");//not a good one, it is hard coded!!!
        }

        //TODO: also you can inject it via an IOC Container
        protected RepositoryBase(IDataContext context)
        {
            Context = context;
        }

        public virtual void Add(TEntity t)
        {
            t.CreationDate = DateTime.Now;

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent(true);
            if (windowsIdentity != null)
                if (windowsIdentity.User != null) 
                    t.CreatedBy = windowsIdentity.User.Value;

            Context.SetEntity<TEntity>().Add(t);
        }

        public List<TEntity> AllToList()
        {
            return Context.SetEntity<TEntity>().ToList();
        }

        public void Attach(TEntity entity)
        {
            if (entity == null)
            {
                //TODO: message from resources
                throw new ArgumentNullException("Attach Entity to ObjectContext hass error");
            }
            Context.SetEntity<TEntity>().Add(entity);
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(System.Linq.Expressions.Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery(criteria).Count();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                //TODO: message from resource bundles
                throw new ArgumentNullException("Error Message");
            }

            entity.ModifiedDate = DateTime.Now;

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent(true);
            if (windowsIdentity != null)
                if (windowsIdentity.User != null)
                    entity.ModifiedBy = windowsIdentity.User.Value;

            Context.SetEntry(entity).State = EntityState.Deleted;
            Context.SetEntity<TEntity>().Remove(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> criteria)
        {
            IEnumerable<TEntity> records = Find(criteria);

            foreach (TEntity record in records)
            {
             //   Delete(record);
                Context.SetEntry(record).State = EntityState.Deleted;
            }
        }

        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> criteria)
        {
            return GetQuery(criteria).AsEnumerable();
        }

        public TEntity First(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }
        public IEnumerable<TEntity> Get<TOrderBy>(System.Linq.Expressions.Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetAll().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetAll().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TOrderBy>(System.Linq.Expressions.Expression<Func<TEntity, bool>> criteria, System.Linq.Expressions.Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            
            return GetQuery(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.SetEntity<TEntity>().AsQueryable();
        }

        public abstract TEntity GetById(TKey id);
   
        #region Implementation of IDisposable

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;

            //auto commit
            Context.SaveAllChanges();
        }
        #endregion Implementation of IDisposable

        public IEnumerable<TEntity> GetEnumerable()
        {
            return GetAll().AsEnumerable();
        }

        public IQueryable<TEntity> GetQuery(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Context.SetEntity<TEntity>().AsQueryable().Where(predicate);
        }

        public TEntity Single(System.Linq.Expressions.Expression<Func<TEntity, bool>> criteria)
        {
            return GetAll().Single(criteria);
        }
        public virtual void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent(true);
            if (windowsIdentity != null)
                if (windowsIdentity.User != null)
                    entity.ModifiedBy = windowsIdentity.User.Value;

            Attach(entity);
            Context.SetEntry(entity).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            Context.SaveAllChanges();
        }
    }
}
