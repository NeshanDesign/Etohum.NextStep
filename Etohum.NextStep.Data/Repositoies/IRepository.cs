using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Etohum.NextStep.Data.Model;

namespace Etohum.NextStep.Data.Repositoies
{
    /// <summary> 
    /// The Repository interface. 
    /// </summary> 
    public interface IRepository<in TKey, TEntity> where TEntity: EntityBase<TKey> where TKey : IComparable
    {
        /// <summary> 
        /// Gets entity by key. 
        /// </summary> 
        /// <typeparam name="TKey">The type of the entity.</typeparam> 
        /// <param name="id">The key value.</param> 
        /// <returns></returns> 
        TEntity GetById(TKey id);

        /// <summary> 
        /// Gets the query. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <returns></returns> 
        IQueryable<TEntity> GetAll();

        /// <summary> 
        /// Gets the query. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="predicate">The predicate.</param> 
        /// <returns></returns> 
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> criteria);

        /// <summary> 
        /// Gets one entity based on matching criteria 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="criteria">The criteria.</param> 
        /// <returns></returns> 
        TEntity Single(Expression<Func<TEntity, bool>> criteria);

        /// <summary> 
        /// Firsts the specified predicate. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="predicate">The predicate.</param> 
        /// <returns></returns> 
        TEntity First(Expression<Func<TEntity, bool>> predicate);

        /// <summary> 
        /// Adds the specified entity. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="entity">The entity.</param> 
        void Add(TEntity entity);

        /// <summary> 
        /// Attaches the specified entity. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="entity">The entity.</param> 
        void Attach(TEntity entity);

        /// <summary> 
        /// Deletes the specified entity. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="entity">The entity.</param> 
        void Delete(TEntity entity);

        /// <summary> 
        /// Deletes one or many entities matching the specified criteria 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="criteria">The criteria.</param> 
        void Delete(Expression<Func<TEntity, bool>> criteria);

        /// <summary> 
        /// Updates changes of the existing entity.  
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="entity">The entity.</param> 
        void Update(TEntity entity);

        /// <summary> 
        /// Gets all. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <returns></returns> 
        IEnumerable<TEntity> GetEnumerable();

        /// <summary> 
        /// Gets the specified order by. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam> 
        /// <param name="orderBy">The order by.</param> 
        /// <param name="pageIndex">Index of the page.</param> 
        /// <param name="pageSize">Size of the page.</param> 
        /// <param name="sortOrder">The sort order.</param> 
        /// <returns></returns> 
        IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending);

        /// <summary> 
        /// Gets the specified criteria. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam> 
        /// <param name="criteria">The criteria.</param> 
        /// <param name="orderBy">The order by.</param> 
        /// <param name="pageIndex">Index of the page.</param> 
        /// <param name="pageSize">Size of the page.</param> 
        /// <param name="sortOrder">The sort order.</param> 
        /// <returns></returns> 
        IEnumerable<TEntity> Get<TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending);

        /// <summary> 
        /// Counts the specified entities. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <returns></returns> 
        int Count();

        /// <summary> 
        /// Counts entities with the specified criteria. 
        /// </summary> 
        /// <typeparam name="TEntity">The type of the entity.</typeparam> 
        /// <param name="criteria">The criteria.</param> 
        /// <returns></returns> 
        int Count(Expression<Func<TEntity, bool>> criteria);
    }     
}
