using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a task within a home improvement project
    /// </summary>
    public class ProjectTask : BaseEntity
    {
        /// <summary>
        /// Title or name of the task
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the task
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status of the task (NotStarted, InProgress, Completed, OnHold, Cancelled)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "NotStarted";

        /// <summary>
        /// Priority level of the task (Low, Medium, High, Critical)
        /// </summary>
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        /// <summary>
        /// Order of the task within the project
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Planned start date of the task
        /// </summary>
        public DateTime? PlannedStartDate { get; set; }

        /// <summary>
        /// Actual start date of the task
        /// </summary>
        public DateTime? ActualStartDate { get; set; }

        /// <summary>
        /// Planned end date of the task
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// Actual end date of the task
        /// </summary>
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// Estimated time to complete the task in hours
        /// </summary>
        public decimal? EstimatedHours { get; set; }

        /// <summary>
        /// Actual time spent on the task in hours
        /// </summary>
        public decimal? ActualHours { get; set; }

        /// <summary>
        /// Estimated cost for this task
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? EstimatedCost { get; set; }

        /// <summary>
        /// Actual cost for this task
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? ActualCost { get; set; }

        /// <summary>
        /// Instructions for completing the task
        /// </summary>
        public string? Instructions { get; set; }

        /// <summary>
        /// Notes about the task
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Assigned person for the task
        /// </summary>
        [StringLength(100)]
        public string? AssignedTo { get; set; }

        /// <summary>
        /// Dependencies - IDs of tasks that must be completed before this task
        /// </summary>
        public string? Dependencies { get; set; }

        /// <summary>
        /// Foreign key to the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Navigation property to the project
        /// </summary>
        public virtual Project Project { get; set; } = null!;
    }
} 