using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;
using HomeMaintenance.Data.Extensions;

namespace HomeMaintenance.Data.Repositories
{
    /// <summary>
    /// Generic repository implementation for data access operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly HomeMaintenanceDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the Repository class
        /// </summary>
        /// <param name="context">The database context</param>
        public Repository(HomeMaintenanceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>Queryable collection of entities</returns>
        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// Gets entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>Queryable collection of filtered entities</returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets the first entity that matches the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>The first matching entity if found, null otherwise</returns>
        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.IsActive = true;

            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        public Task<T> UpdateAsync(T entity)
        {
            entity.ModifiedAt = DateTime.UtcNow;

            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            return await DeleteAsync(entity);
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if deleted, false otherwise</returns>
        public Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                return Task.FromResult(false);

            entity.IsActive = false;
            entity.ModifiedAt = DateTime.UtcNow;

            _dbSet.Update(entity);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Checks if an entity exists with the specified ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>True if exists, false otherwise</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && e.IsActive);
        }

        /// <summary>
        /// Gets the count of entities
        /// </summary>
        /// <returns>The count of entities</returns>
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync(e => e.IsActive);
        }

        /// <summary>
        /// Gets the count of entities that match the specified predicate
        /// </summary>
        /// <param name="predicate">The predicate to filter entities</param>
        /// <returns>The count of filtered entities</returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate.And(e => e.IsActive));
        }
    }
} 