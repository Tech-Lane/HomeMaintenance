using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Services
{
    /// <summary>
    /// Service implementation for maintenance task-related business operations
    /// </summary>
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the MaintenanceService class
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance</param>
        public MaintenanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Gets all maintenance tasks
        /// </summary>
        /// <returns>Collection of maintenance tasks</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetAllMaintenanceTasksAsync()
        {
            var tasks = await _unitOfWork.MaintenanceTasks.GetAll()
                .Where(t => t.IsActive)
                .ToListAsync();
            return tasks;
        }

        /// <summary>
        /// Gets a maintenance task by its ID
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>The maintenance task if found, null otherwise</returns>
        public async Task<MaintenanceTask?> GetMaintenanceTaskByIdAsync(int id)
        {
            return await _unitOfWork.MaintenanceTasks.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets overdue maintenance tasks
        /// </summary>
        /// <returns>Collection of overdue maintenance tasks</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetOverdueMaintenanceTasksAsync()
        {
            var now = DateTime.UtcNow;
            var tasks = await _unitOfWork.MaintenanceTasks.Get(t => 
                t.IsActive && 
                t.NextDueDate.HasValue && 
                t.NextDueDate < now)
                .ToListAsync();
            return tasks;
        }

        /// <summary>
        /// Gets upcoming maintenance tasks
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead</param>
        /// <returns>Collection of upcoming maintenance tasks</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetUpcomingMaintenanceTasksAsync(int daysAhead = 7)
        {
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(daysAhead);
            var tasks = await _unitOfWork.MaintenanceTasks.Get(t => 
                t.IsActive && 
                t.NextDueDate.HasValue && 
                t.NextDueDate >= startDate && 
                t.NextDueDate <= endDate)
                .ToListAsync();
            return tasks;
        }

        /// <summary>
        /// Gets maintenance tasks by priority
        /// </summary>
        /// <param name="priority">The priority level</param>
        /// <returns>Collection of maintenance tasks with the specified priority</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksByPriorityAsync(string priority)
        {
            var tasks = await _unitOfWork.MaintenanceTasks.Get(t => 
                t.IsActive && t.Priority == priority)
                .ToListAsync();
            return tasks;
        }

        /// <summary>
        /// Creates a new maintenance task
        /// </summary>
        /// <param name="task">The maintenance task to create</param>
        /// <returns>The created maintenance task</returns>
        public async Task<MaintenanceTask> CreateMaintenanceTaskAsync(MaintenanceTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            // Calculate next due date if not provided
            if (!task.NextDueDate.HasValue)
            {
                task.NextDueDate = CalculateNextDueDate(task);
            }

            var createdTask = await _unitOfWork.MaintenanceTasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return createdTask;
        }

        /// <summary>
        /// Updates an existing maintenance task
        /// </summary>
        /// <param name="task">The maintenance task to update</param>
        /// <returns>The updated maintenance task</returns>
        public async Task<MaintenanceTask> UpdateMaintenanceTaskAsync(MaintenanceTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var existingTask = await _unitOfWork.MaintenanceTasks.GetByIdAsync(task.Id);
            if (existingTask == null)
                throw new InvalidOperationException($"Maintenance task with ID {task.Id} not found");

            var updatedTask = await _unitOfWork.MaintenanceTasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return updatedTask;
        }

        /// <summary>
        /// Deletes a maintenance task
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        public async Task<bool> DeleteMaintenanceTaskAsync(int id)
        {
            var result = await _unitOfWork.MaintenanceTasks.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Marks a maintenance task as completed
        /// </summary>
        /// <param name="taskId">The task ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated maintenance task</returns>
        public async Task<MaintenanceTask> CompleteMaintenanceTaskAsync(int taskId, TaskCompletion completion)
        {
            var task = await _unitOfWork.MaintenanceTasks.GetByIdAsync(taskId);
            if (task == null)
                throw new InvalidOperationException($"Maintenance task with ID {taskId} not found");

            // Update task completion details
            task.LastCompletedDate = completion.CompletedAt;
            task.NextDueDate = CalculateNextDueDate(task);

            // Save the completion record
            completion.MaintenanceTaskId = taskId;
            await _unitOfWork.Repository<TaskCompletion>().AddAsync(completion);

            // Update the task
            var updatedTask = await _unitOfWork.MaintenanceTasks.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return updatedTask;
        }

        /// <summary>
        /// Calculates the next due date for a maintenance task
        /// </summary>
        /// <param name="task">The maintenance task</param>
        /// <returns>The calculated next due date</returns>
        public DateTime CalculateNextDueDate(MaintenanceTask task)
        {
            var baseDate = task.LastCompletedDate ?? DateTime.UtcNow;
            
            return task.Frequency.ToLower() switch
            {
                "daily" => baseDate.AddDays(task.FrequencyInterval),
                "weekly" => baseDate.AddDays(7 * task.FrequencyInterval),
                "monthly" => baseDate.AddMonths(task.FrequencyInterval),
                "yearly" => baseDate.AddYears(task.FrequencyInterval),
                _ => baseDate.AddDays(task.FrequencyInterval) // Default to daily
            };
        }

        /// <summary>
        /// Gets maintenance tasks that need reminders
        /// </summary>
        /// <returns>Collection of maintenance tasks that need reminders</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksNeedingRemindersAsync()
        {
            var now = DateTime.UtcNow;
            var tasks = await _unitOfWork.MaintenanceTasks.Get(t => 
                t.IsActive && 
                t.SendReminders && 
                t.NextDueDate.HasValue && 
                t.NextDueDate <= now.AddDays(t.ReminderDaysBefore))
                .ToListAsync();
            return tasks;
        }
    }
} 