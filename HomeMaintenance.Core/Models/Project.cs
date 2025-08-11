using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a home improvement project
    /// </summary>
    public class Project : BaseEntity
    {
        /// <summary>
        /// Title or name of the project
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the project
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category of the project (e.g., Kitchen, Bathroom, Exterior, Interior)
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Status of the project (Planning, InProgress, OnHold, Completed, Cancelled)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Planning";

        /// <summary>
        /// Priority level of the project (Low, Medium, High, Critical)
        /// </summary>
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        /// <summary>
        /// Planned start date of the project
        /// </summary>
        public DateTime? PlannedStartDate { get; set; }

        /// <summary>
        /// Actual start date of the project
        /// </summary>
        public DateTime? ActualStartDate { get; set; }

        /// <summary>
        /// Planned end date of the project
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// Actual end date of the project
        /// </summary>
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// Estimated budget for the project
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? EstimatedBudget { get; set; }

        /// <summary>
        /// Actual cost of the project
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? ActualCost { get; set; }

        /// <summary>
        /// Location where the project is being done
        /// </summary>
        [StringLength(200)]
        public string? Location { get; set; }

        /// <summary>
        /// Notes about the project
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Photos of the project (comma-separated URLs)
        /// </summary>
        public string? PhotoUrls { get; set; }

        /// <summary>
        /// Collection of tasks within this project
        /// </summary>
        public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

        /// <summary>
        /// Collection of cost estimates for this project
        /// </summary>
        public virtual ICollection<CostEstimate> CostEstimates { get; set; } = new List<CostEstimate>();
    }
} 