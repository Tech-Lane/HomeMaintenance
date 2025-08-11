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
    /// Service implementation for chore-related business operations
    /// </summary>
    public class ChoreService : IChoreService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the ChoreService class
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance</param>
        public ChoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Gets all chores
        /// </summary>
        /// <returns>Collection of chores</returns>
        public async Task<IEnumerable<Chore>> GetAllChoresAsync()
        {
            var chores = await _unitOfWork.Chores.GetAll()
                .Where(c => c.IsActive)
                .ToListAsync();
            return chores;
        }

        /// <summary>
        /// Gets a chore by its ID
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>The chore if found, null otherwise</returns>
        public async Task<Chore?> GetChoreByIdAsync(int id)
        {
            return await _unitOfWork.Chores.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets overdue chores
        /// </summary>
        /// <returns>Collection of overdue chores</returns>
        public async Task<IEnumerable<Chore>> GetOverdueChoresAsync()
        {
            var now = DateTime.UtcNow;
            var chores = await _unitOfWork.Chores.Get(c => 
                c.IsActive && 
                c.DueDate.HasValue && 
                c.DueDate < now)
                .ToListAsync();
            return chores;
        }

        /// <summary>
        /// Gets upcoming chores
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead</param>
        /// <returns>Collection of upcoming chores</returns>
        public async Task<IEnumerable<Chore>> GetUpcomingChoresAsync(int daysAhead = 7)
        {
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(daysAhead);
            var chores = await _unitOfWork.Chores.Get(c => 
                c.IsActive && 
                c.DueDate.HasValue && 
                c.DueDate >= startDate && 
                c.DueDate <= endDate)
                .ToListAsync();
            return chores;
        }

        /// <summary>
        /// Gets chores by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of chores in the specified category</returns>
        public async Task<IEnumerable<Chore>> GetChoresByCategoryAsync(string category)
        {
            var chores = await _unitOfWork.Chores.Get(c => 
                c.IsActive && c.Category == category)
                .ToListAsync();
            return chores;
        }

        /// <summary>
        /// Gets chores by assigned person
        /// </summary>
        /// <param name="assignedTo">The assigned person</param>
        /// <returns>Collection of chores assigned to the specified person</returns>
        public async Task<IEnumerable<Chore>> GetChoresByAssignedPersonAsync(string assignedTo)
        {
            var chores = await _unitOfWork.Chores.Get(c => 
                c.IsActive && c.AssignedTo == assignedTo)
                .ToListAsync();
            return chores;
        }

        /// <summary>
        /// Creates a new chore
        /// </summary>
        /// <param name="chore">The chore to create</param>
        /// <returns>The created chore</returns>
        public async Task<Chore> CreateChoreAsync(Chore chore)
        {
            if (chore == null)
                throw new ArgumentNullException(nameof(chore));

            // Calculate next due date if not provided and frequency is set
            if (!chore.DueDate.HasValue && !string.IsNullOrEmpty(chore.Frequency))
            {
                chore.DueDate = CalculateNextDueDate(chore);
            }

            var createdChore = await _unitOfWork.Chores.AddAsync(chore);
            await _unitOfWork.SaveChangesAsync();
            return createdChore;
        }

        /// <summary>
        /// Updates an existing chore
        /// </summary>
        /// <param name="chore">The chore to update</param>
        /// <returns>The updated chore</returns>
        public async Task<Chore> UpdateChoreAsync(Chore chore)
        {
            if (chore == null)
                throw new ArgumentNullException(nameof(chore));

            var existingChore = await _unitOfWork.Chores.GetByIdAsync(chore.Id);
            if (existingChore == null)
                throw new InvalidOperationException($"Chore with ID {chore.Id} not found");

            var updatedChore = await _unitOfWork.Chores.UpdateAsync(chore);
            await _unitOfWork.SaveChangesAsync();
            return updatedChore;
        }

        /// <summary>
        /// Deletes a chore
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        public async Task<bool> DeleteChoreAsync(int id)
        {
            var result = await _unitOfWork.Chores.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Marks a chore as completed
        /// </summary>
        /// <param name="choreId">The chore ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated chore</returns>
        public async Task<Chore> CompleteChoreAsync(int choreId, ChoreCompletion completion)
        {
            var chore = await _unitOfWork.Chores.GetByIdAsync(choreId);
            if (chore == null)
                throw new InvalidOperationException($"Chore with ID {choreId} not found");

            // Update chore completion details
            chore.LastCompletedDate = completion.CompletedAt;
            
            // Calculate next due date if this is a recurring chore
            if (!string.IsNullOrEmpty(chore.Frequency))
            {
                chore.NextDueDate = CalculateNextDueDate(chore);
            }

            // Save the completion record
            completion.ChoreId = choreId;
            await _unitOfWork.Repository<ChoreCompletion>().AddAsync(completion);

            // Update the chore
            var updatedChore = await _unitOfWork.Chores.UpdateAsync(chore);
            await _unitOfWork.SaveChangesAsync();
            return updatedChore;
        }

        /// <summary>
        /// Calculates the next due date for a chore
        /// </summary>
        /// <param name="chore">The chore</param>
        /// <returns>The calculated next due date</returns>
        public DateTime CalculateNextDueDate(Chore chore)
        {
            var baseDate = chore.LastCompletedDate ?? DateTime.UtcNow;
            
            return chore.Frequency?.ToLower() switch
            {
                "daily" => baseDate.AddDays(chore.FrequencyInterval ?? 1),
                "weekly" => baseDate.AddDays(7 * (chore.FrequencyInterval ?? 1)),
                "monthly" => baseDate.AddMonths(chore.FrequencyInterval ?? 1),
                "yearly" => baseDate.AddYears(chore.FrequencyInterval ?? 1),
                _ => baseDate.AddDays(chore.FrequencyInterval ?? 1) // Default to daily
            };
        }
    }
} 