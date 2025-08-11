using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Interfaces
{
    /// <summary>
    /// Service interface for chore-related business operations
    /// </summary>
    public interface IChoreService
    {
        /// <summary>
        /// Gets all chores
        /// </summary>
        /// <returns>Collection of chores</returns>
        Task<IEnumerable<Chore>> GetAllChoresAsync();

        /// <summary>
        /// Gets a chore by its ID
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>The chore if found, null otherwise</returns>
        Task<Chore?> GetChoreByIdAsync(int id);

        /// <summary>
        /// Gets overdue chores
        /// </summary>
        /// <returns>Collection of overdue chores</returns>
        Task<IEnumerable<Chore>> GetOverdueChoresAsync();

        /// <summary>
        /// Gets upcoming chores
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead</param>
        /// <returns>Collection of upcoming chores</returns>
        Task<IEnumerable<Chore>> GetUpcomingChoresAsync(int daysAhead = 7);

        /// <summary>
        /// Gets chores by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of chores in the specified category</returns>
        Task<IEnumerable<Chore>> GetChoresByCategoryAsync(string category);

        /// <summary>
        /// Gets chores by assigned person
        /// </summary>
        /// <param name="assignedTo">The assigned person</param>
        /// <returns>Collection of chores assigned to the specified person</returns>
        Task<IEnumerable<Chore>> GetChoresByAssignedPersonAsync(string assignedTo);

        /// <summary>
        /// Creates a new chore
        /// </summary>
        /// <param name="chore">The chore to create</param>
        /// <returns>The created chore</returns>
        Task<Chore> CreateChoreAsync(Chore chore);

        /// <summary>
        /// Updates an existing chore
        /// </summary>
        /// <param name="chore">The chore to update</param>
        /// <returns>The updated chore</returns>
        Task<Chore> UpdateChoreAsync(Chore chore);

        /// <summary>
        /// Deletes a chore
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        Task<bool> DeleteChoreAsync(int id);

        /// <summary>
        /// Marks a chore as completed
        /// </summary>
        /// <param name="choreId">The chore ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated chore</returns>
        Task<Chore> CompleteChoreAsync(int choreId, ChoreCompletion completion);

        /// <summary>
        /// Calculates the next due date for a chore
        /// </summary>
        /// <param name="chore">The chore</param>
        /// <returns>The calculated next due date</returns>
        DateTime CalculateNextDueDate(Chore chore);
    }
} 