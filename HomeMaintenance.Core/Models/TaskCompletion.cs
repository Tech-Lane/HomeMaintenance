using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a completion record for a maintenance task
    /// </summary>
    public class TaskCompletion : BaseEntity
    {
        /// <summary>
        /// Date and time when the task was completed
        /// </summary>
        [Required]
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notes about the completion
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Actual time spent completing the task in minutes
        /// </summary>
        public int? ActualTimeMinutes { get; set; }

        /// <summary>
        /// Actual cost incurred to complete the task
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? ActualCost { get; set; }

        /// <summary>
        /// Who completed the task
        /// </summary>
        [StringLength(100)]
        public string? CompletedBy { get; set; }

        /// <summary>
        /// Rating of the task difficulty (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? DifficultyRating { get; set; }

        /// <summary>
        /// Photos taken during completion (comma-separated URLs)
        /// </summary>
        public string? PhotoUrls { get; set; }

        /// <summary>
        /// Foreign key to the maintenance task
        /// </summary>
        public int MaintenanceTaskId { get; set; }

        /// <summary>
        /// Navigation property to the maintenance task
        /// </summary>
        public virtual MaintenanceTask MaintenanceTask { get; set; } = null!;
    }
} 