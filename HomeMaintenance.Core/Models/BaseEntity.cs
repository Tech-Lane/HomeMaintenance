using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Base class for all domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the entity was last modified
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Indicates whether the entity is active/deleted
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 