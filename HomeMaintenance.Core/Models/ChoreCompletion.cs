using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a completion record for a chore
    /// </summary>
    public class ChoreCompletion : BaseEntity
    {
        /// <summary>
        /// Date and time when the chore was completed
        /// </summary>
        [Required]
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notes about the completion
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Actual time spent completing the chore in minutes
        /// </summary>
        public int? ActualTimeMinutes { get; set; }

        /// <summary>
        /// Who completed the chore
        /// </summary>
        [StringLength(100)]
        public string? CompletedBy { get; set; }

        /// <summary>
        /// Rating of the chore difficulty (1-5)
        /// </summary>
        [Range(1, 5)]
        public int? DifficultyRating { get; set; }

        /// <summary>
        /// Whether the chore was completed on time
        /// </summary>
        public bool CompletedOnTime { get; set; } = true;

        /// <summary>
        /// Foreign key to the chore
        /// </summary>
        public int ChoreId { get; set; }

        /// <summary>
        /// Navigation property to the chore
        /// </summary>
        public virtual Chore Chore { get; set; } = null!;
    }
} 