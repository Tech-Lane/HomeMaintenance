using System;
using System.Threading.Tasks;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Interfaces
{
    /// <summary>
    /// Unit of work interface for managing transactions and repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for the specified entity type
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>The repository instance</returns>
        IRepository<T> Repository<T>() where T : BaseEntity;

        /// <summary>
        /// Gets the appliance repository
        /// </summary>
        IRepository<Appliance> Appliances { get; }

        /// <summary>
        /// Gets the maintenance task repository
        /// </summary>
        IRepository<MaintenanceTask> MaintenanceTasks { get; }

        /// <summary>
        /// Gets the chore repository
        /// </summary>
        IRepository<Chore> Chores { get; }

        /// <summary>
        /// Gets the project repository
        /// </summary>
        IRepository<Project> Projects { get; }

        /// <summary>
        /// Gets the project task repository
        /// </summary>
        IRepository<ProjectTask> ProjectTasks { get; }

        /// <summary>
        /// Gets the cost estimate repository
        /// </summary>
        IRepository<CostEstimate> CostEstimates { get; }

        /// <summary>
        /// Gets the floor plan repository
        /// </summary>
        IRepository<FloorPlan> FloorPlans { get; }

        /// <summary>
        /// Saves all changes made in this unit of work
        /// </summary>
        /// <returns>The number of affected records</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        /// <returns>The transaction object</returns>
        Task<IDisposable> BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
} 