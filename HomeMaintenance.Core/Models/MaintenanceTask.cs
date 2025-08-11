using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a recurring maintenance task
    /// </summary>
    public class MaintenanceTask : BaseEntity
    {
        /// <summary>
        /// Title or name of the maintenance task
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the maintenance task
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Frequency of the maintenance task (e.g., Daily, Weekly, Monthly, Yearly)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Interval value for the frequency (e.g., every 3 months)
        /// </summary>
        public int FrequencyInterval { get; set; } = 1;

        /// <summary>
        /// Date when the task was last completed
        /// </summary>
        public DateTime? LastCompletedDate { get; set; }

        /// <summary>
        /// Date when the task is next due
        /// </summary>
        public DateTime? NextDueDate { get; set; }

        /// <summary>
        /// Priority level of the task (Low, Medium, High, Critical)
        /// </summary>
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        /// <summary>
        /// Estimated time to complete the task in minutes
        /// </summary>
        public int? EstimatedTimeMinutes { get; set; }

        /// <summary>
        /// Estimated cost to complete the task
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? EstimatedCost { get; set; }

        /// <summary>
        /// Instructions for completing the task
        /// </summary>
        public string? Instructions { get; set; }



        /// <summary>
        /// Whether to send reminders for this task
        /// </summary>
        public bool SendReminders { get; set; } = true;

        /// <summary>
        /// Number of days before due date to send reminder
        /// </summary>
        public int ReminderDaysBefore { get; set; } = 7;

        /// <summary>
        /// Foreign key to the associated appliance (optional)
        /// </summary>
        public int? ApplianceId { get; set; }

        /// <summary>
        /// Navigation property to the associated appliance
        /// </summary>
        public virtual Appliance? Appliance { get; set; }

        /// <summary>
        /// Collection of task completion records
        /// </summary>
        public virtual ICollection<TaskCompletion> Completions { get; set; } = new List<TaskCompletion>();
    }
} 