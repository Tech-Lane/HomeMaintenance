using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a household chore (one-time or recurring)
    /// </summary>
    public class Chore : BaseEntity
    {
        /// <summary>
        /// Title or name of the chore
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the chore
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category of the chore (e.g., Kitchen, Bathroom, Laundry, Outdoor)
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Frequency of the chore (e.g., Daily, Weekly, Monthly, Yearly, OneTime)
        /// </summary>
        [StringLength(20)]
        public string? Frequency { get; set; }

        /// <summary>
        /// Interval value for the frequency (e.g., every 3 days)
        /// </summary>
        public int? FrequencyInterval { get; set; }

        /// <summary>
        /// Date when the chore is due
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Date when the chore was last completed
        /// </summary>
        public DateTime? LastCompletedDate { get; set; }

        /// <summary>
        /// Date when the chore is next due
        /// </summary>
        public DateTime? NextDueDate { get; set; }

        /// <summary>
        /// Priority level of the chore (Low, Medium, High, Critical)
        /// </summary>
        [StringLength(20)]
        public string Priority { get; set; } = "Medium";

        /// <summary>
        /// Estimated time to complete the chore in minutes
        /// </summary>
        public int? EstimatedTimeMinutes { get; set; }



        /// <summary>
        /// Whether to send reminders for this chore
        /// </summary>
        public bool SendReminders { get; set; } = true;

        /// <summary>
        /// Number of days before due date to send reminder
        /// </summary>
        public int ReminderDaysBefore { get; set; } = 1;

        /// <summary>
        /// Assigned person for the chore
        /// </summary>
        [StringLength(100)]
        public string? AssignedTo { get; set; }

        /// <summary>
        /// Collection of chore completion records
        /// </summary>
        public virtual ICollection<ChoreCompletion> Completions { get; set; } = new List<ChoreCompletion>();
    }
} 