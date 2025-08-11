using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Interfaces
{
    /// <summary>
    /// Service interface for maintenance task-related business operations
    /// </summary>
    public interface IMaintenanceService
    {
        /// <summary>
        /// Gets all maintenance tasks
        /// </summary>
        /// <returns>Collection of maintenance tasks</returns>
        Task<IEnumerable<MaintenanceTask>> GetAllMaintenanceTasksAsync();

        /// <summary>
        /// Gets a maintenance task by its ID
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>The maintenance task if found, null otherwise</returns>
        Task<MaintenanceTask?> GetMaintenanceTaskByIdAsync(int id);

        /// <summary>
        /// Gets overdue maintenance tasks
        /// </summary>
        /// <returns>Collection of overdue maintenance tasks</returns>
        Task<IEnumerable<MaintenanceTask>> GetOverdueMaintenanceTasksAsync();

        /// <summary>
        /// Gets upcoming maintenance tasks
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead</param>
        /// <returns>Collection of upcoming maintenance tasks</returns>
        Task<IEnumerable<MaintenanceTask>> GetUpcomingMaintenanceTasksAsync(int daysAhead = 7);

        /// <summary>
        /// Gets maintenance tasks by priority
        /// </summary>
        /// <param name="priority">The priority level</param>
        /// <returns>Collection of maintenance tasks with the specified priority</returns>
        Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByPriorityAsync(string priority);

        /// <summary>
        /// Creates a new maintenance task
        /// </summary>
        /// <param name="task">The maintenance task to create</param>
        /// <returns>The created maintenance task</returns>
        Task<MaintenanceTask> CreateMaintenanceTaskAsync(MaintenanceTask task);

        /// <summary>
        /// Updates an existing maintenance task
        /// </summary>
        /// <param name="task">The maintenance task to update</param>
        /// <returns>The updated maintenance task</returns>
        Task<MaintenanceTask> UpdateMaintenanceTaskAsync(MaintenanceTask task);

        /// <summary>
        /// Deletes a maintenance task
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        Task<bool> DeleteMaintenanceTaskAsync(int id);

        /// <summary>
        /// Marks a maintenance task as completed
        /// </summary>
        /// <param name="taskId">The task ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated maintenance task</returns>
        Task<MaintenanceTask> CompleteMaintenanceTaskAsync(int taskId, TaskCompletion completion);

        /// <summary>
        /// Calculates the next due date for a maintenance task
        /// </summary>
        /// <param name="task">The maintenance task</param>
        /// <returns>The calculated next due date</returns>
        DateTime CalculateNextDueDate(MaintenanceTask task);

        /// <summary>
        /// Gets maintenance tasks that need reminders
        /// </summary>
        /// <returns>Collection of maintenance tasks that need reminders</returns>
        Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksNeedingRemindersAsync();
    }
} 