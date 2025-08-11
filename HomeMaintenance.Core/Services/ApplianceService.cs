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
    /// Service implementation for appliance-related business operations
    /// </summary>
    public class ApplianceService : IApplianceService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the ApplianceService class
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance</param>
        public ApplianceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Gets all appliances
        /// </summary>
        /// <returns>Collection of appliances</returns>
        public async Task<IEnumerable<Appliance>> GetAllAppliancesAsync()
        {
            var appliances = await _unitOfWork.Appliances.GetAll()
                .Where(a => a.IsActive)
                .ToListAsync();
            return appliances;
        }

        /// <summary>
        /// Gets an appliance by its ID
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>The appliance if found, null otherwise</returns>
        public async Task<Appliance?> GetApplianceByIdAsync(int id)
        {
            return await _unitOfWork.Appliances.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets appliances by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of appliances in the specified category</returns>
        public async Task<IEnumerable<Appliance>> GetAppliancesByCategoryAsync(string category)
        {
            var appliances = await _unitOfWork.Appliances.Get(a => a.Category == category && a.IsActive)
                .ToListAsync();
            return appliances;
        }

        /// <summary>
        /// Gets appliances with expiring warranties
        /// </summary>
        /// <param name="daysThreshold">Number of days to check ahead</param>
        /// <returns>Collection of appliances with expiring warranties</returns>
        public async Task<IEnumerable<Appliance>> GetAppliancesWithExpiringWarrantiesAsync(int daysThreshold = 30)
        {
            var expirationDate = DateTime.UtcNow.AddDays(daysThreshold);
            var appliances = await _unitOfWork.Appliances.Get(a => 
                a.IsActive && 
                a.WarrantyExpirationDate.HasValue && 
                a.WarrantyExpirationDate <= expirationDate)
                .ToListAsync();
            return appliances;
        }

        /// <summary>
        /// Creates a new appliance
        /// </summary>
        /// <param name="appliance">The appliance to create</param>
        /// <returns>The created appliance</returns>
        public async Task<Appliance> CreateApplianceAsync(Appliance appliance)
        {
            if (appliance == null)
                throw new ArgumentNullException(nameof(appliance));

            var createdAppliance = await _unitOfWork.Appliances.AddAsync(appliance);
            await _unitOfWork.SaveChangesAsync();
            return createdAppliance;
        }

        /// <summary>
        /// Updates an existing appliance
        /// </summary>
        /// <param name="appliance">The appliance to update</param>
        /// <returns>The updated appliance</returns>
        public async Task<Appliance> UpdateApplianceAsync(Appliance appliance)
        {
            if (appliance == null)
                throw new ArgumentNullException(nameof(appliance));

            var existingAppliance = await _unitOfWork.Appliances.GetByIdAsync(appliance.Id);
            if (existingAppliance == null)
                throw new InvalidOperationException($"Appliance with ID {appliance.Id} not found");

            var updatedAppliance = await _unitOfWork.Appliances.UpdateAsync(appliance);
            await _unitOfWork.SaveChangesAsync();
            return updatedAppliance;
        }

        /// <summary>
        /// Deletes an appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>True if deleted, false otherwise</returns>
        public async Task<bool> DeleteApplianceAsync(int id)
        {
            var result = await _unitOfWork.Appliances.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Gets maintenance tasks for an appliance
        /// </summary>
        /// <param name="applianceId">The appliance ID</param>
        /// <returns>Collection of maintenance tasks</returns>
        public async Task<IEnumerable<MaintenanceTask>> GetMaintenanceTasksForApplianceAsync(int applianceId)
        {
            var tasks = await _unitOfWork.MaintenanceTasks.Get(t => 
                t.ApplianceId == applianceId && t.IsActive)
                .ToListAsync();
            return tasks;
        }

        /// <summary>
        /// Gets manuals for an appliance
        /// </summary>
        /// <param name="applianceId">The appliance ID</param>
        /// <returns>Collection of appliance manuals</returns>
        public async Task<IEnumerable<ApplianceManual>> GetManualsForApplianceAsync(int applianceId)
        {
            var manuals = await _unitOfWork.Repository<ApplianceManual>().Get(m => 
                m.ApplianceId == applianceId && m.IsActive)
                .ToListAsync();
            return manuals;
        }
    }
} 