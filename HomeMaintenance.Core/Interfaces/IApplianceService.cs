using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Core.Interfaces
{
    /// <summary>
    /// Service interface for appliance-related business operations
    /// </summary>
    public interface IApplianceService
    {
        /// <summary>
        /// Gets all appliances
        /// </summary>
        /// <returns>Collection of appliances</returns>
        Task<IEnumerable<Appliance>> GetAllAppliancesAsync();

        /// <summary>
        /// Gets an appliance by its ID
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>The appliance if found, null otherwise</returns>
        Task<Appliance?> GetApplianceByIdAsync(int id);

        /// <summary>
        /// Gets appliances by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of appliances in the specified category</returns>
        Task<IEnumerable<Appliance>> GetAppliancesByCategoryAsync(string category);

        /// <summary>
        /// Gets appliances with expiring warranties
        /// </summary>
        /// <param name="daysThreshold">Number of days to check ahead</param>
        /// <returns>Collection of appliances with expiring warranties</returns>
        Task<IEnumerable<Appliance>> GetAppliancesWithExpiringWarrantiesAsync(int daysThreshold = 30);

        /// <summary>
        /// Creates a new appliance
        /// </summary>
        /// <param name="appliance">The appliance to create</param>
        /// <returns>The created appliance</returns>
        Task<Appliance> CreateApplianceAsync(Appliance appliance);

        /// <summary>
        /// Updates an existing appliance
        /// </summary>
        /// <param name="appliance">The appliance to update</param>
        /// <returns>The updated appliance</returns>
        Task<Appliance> UpdateApplianceAsync(Appliance appliance);

        /// <summary>
        /// Deletes an appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        Task<bool> DeleteApplianceAsync(int id);

        /// <summary>
        /// Gets maintenance tasks for an appliance
        /// </summary>
        /// <param name="applianceId">The appliance ID</param>
        /// <returns>Collection of maintenance tasks</returns>
        Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksForApplianceAsync(int applianceId);

        /// <summary>
        /// Gets manuals for an appliance
        /// </summary>
        /// <param name="applianceId">The appliance ID</param>
        /// <returns>Collection of appliance manuals</returns>
        Task<IEnumerable<ApplianceManual>> GetManualsForApplianceAsync(int applianceId);
    }
} 