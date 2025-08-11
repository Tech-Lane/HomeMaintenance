using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a home appliance with maintenance and warranty information
    /// </summary>
    public class Appliance : BaseEntity
    {
        /// <summary>
        /// Name or description of the appliance
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Brand or manufacturer of the appliance
        /// </summary>
        [StringLength(100)]
        public string? Brand { get; set; }

        /// <summary>
        /// Model number of the appliance
        /// </summary>
        [StringLength(100)]
        public string? Model { get; set; }

        /// <summary>
        /// Serial number of the appliance
        /// </summary>
        [StringLength(100)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Date when the appliance was purchased
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Purchase price of the appliance
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Location of the appliance in the home
        /// </summary>
        [StringLength(200)]
        public string? Location { get; set; }

        /// <summary>
        /// Category or type of appliance (e.g., Kitchen, Laundry, HVAC)
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Warranty expiration date
        /// </summary>
        public DateTime? WarrantyExpirationDate { get; set; }

        /// <summary>
        /// Extended warranty expiration date
        /// </summary>
        public DateTime? ExtendedWarrantyExpirationDate { get; set; }

        /// <summary>
        /// Notes about the appliance
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// URL or path to the appliance manual
        /// </summary>
        [StringLength(500)]
        public string? ManualUrl { get; set; }

        /// <summary>
        /// URL or path to the appliance photo
        /// </summary>
        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Collection of maintenance tasks associated with this appliance
        /// </summary>
        public virtual ICollection<MaintenanceTask> MaintenanceTasks { get; set; } = new List<MaintenanceTask>();

        /// <summary>
        /// Collection of appliance manuals
        /// </summary>
        public virtual ICollection<ApplianceManual> Manuals { get; set; } = new List<ApplianceManual>();
    }
} 