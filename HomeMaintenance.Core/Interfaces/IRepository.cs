using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Queryable collection of entities</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>Queryable collection of filtered entities</returns>
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets the first entity that matches the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>The first matching entity if found, null otherwise</returns>
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if deleted, false otherwise</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Checks if an entity exists with the specified ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Gets the count of entities
        /// </summary>
        /// <returns>The count of entities</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets the count of entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>The count of filtered entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
} 