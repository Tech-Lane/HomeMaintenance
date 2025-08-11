using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;
using HomeMaintenance.Data.Repositories;

namespace HomeMaintenance.Data
{
    /// <summary>
    /// Unit of work implementation for managing transactions and repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeMaintenanceDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        // Repository instances
        private IRepository<Appliance>? _appliances;
        private IRepository<MaintenanceTask>? _maintenanceTasks;
        private IRepository<Chore>? _chores;
        private IRepository<Project>? _projects;
        private IRepository<ProjectTask>? _projectTasks;
        private IRepository<CostEstimate>? _costEstimates;
        private IRepository<FloorPlan>? _floorPlans;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class
        /// </summary>
        /// <param name="context">The database context</param>
        public UnitOfWork(HomeMaintenanceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the repository for the specified entity type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>The repository instance</returns>
        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            return new Repository<T>(_context);
        }

        /// <summary>
        /// Gets the appliance repository
        /// </summary>
        public IRepository<Appliance> Appliances
        {
            get
            {
                _appliances ??= new Repository<Appliance>(_context);
                return _appliances;
            }
        }

        /// <summary>
        /// Gets the maintenance task repository
        /// </summary>
        public IRepository<MaintenanceTask> MaintenanceTasks
        {
            get
            {
                _maintenanceTasks ??= new Repository<MaintenanceTask>(_context);
                return _maintenanceTasks;
            }
        }

        /// <summary>
        /// Gets the chore repository
        /// </summary>
        public IRepository<Chore> Chores
        {
            get
            {
                _chores ??= new Repository<Chore>(_context);
                return _chores;
            }
        }

        /// <summary>
        /// Gets the project repository
        /// </summary>
        public IRepository<Project> Projects
        {
            get
            {
                _projects ??= new Repository<Project>(_context);
                return _projects;
            }
        }

        /// <summary>
        /// Gets the project task repository
        /// </summary>
        public IRepository<ProjectTask> ProjectTasks
        {
            get
            {
                _projectTasks ??= new Repository<ProjectTask>(_context);
                return _projectTasks;
            }
        }

        /// <summary>
        /// Gets the cost estimate repository
        /// </summary>
        public IRepository<CostEstimate> CostEstimates
        {
            get
            {
                _costEstimates ??= new Repository<CostEstimate>(_context);
                return _costEstimates;
            }
        }

        /// <summary>
        /// Gets the floor plan repository
        /// </summary>
        public IRepository<FloorPlan> FloorPlans
        {
            get
            {
                _floorPlans ??= new Repository<FloorPlan>(_context);
                return _floorPlans;
            }
        }

        /// <summary>
        /// Saves all changes made in this unit of work
        /// </summary>
        /// <returns>The number of affected records</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        /// <returns>The transaction object</returns>
        public async Task<IDisposable> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Disposes the unit of work
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the unit of work
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
            }
            _disposed = true;
        }
    }
} 