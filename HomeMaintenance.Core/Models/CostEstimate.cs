using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a cost estimate for a project or repair
    /// </summary>
    public class CostEstimate : BaseEntity
    {
        /// <summary>
        /// Title or description of the cost estimate
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the cost estimate
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category of the cost (e.g., Materials, Labor, Equipment, Permits)
        /// </summary>
        [StringLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Estimated cost amount
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal EstimatedAmount { get; set; }

        /// <summary>
        /// Actual cost amount (if known)
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? ActualAmount { get; set; }

        /// <summary>
        /// Currency of the cost estimate
        /// </summary>
        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Date when the estimate was created
        /// </summary>
        public DateTime EstimateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the estimate expires
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Source of the estimate (e.g., Contractor, DIY, Online)
        /// </summary>
        [StringLength(100)]
        public string? Source { get; set; }

        /// <summary>
        /// Contact information for the source
        /// </summary>
        [StringLength(200)]
        public string? ContactInfo { get; set; }

        /// <summary>
        /// Notes about the estimate
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Whether this is the preferred estimate
        /// </summary>
        public bool IsPreferred { get; set; } = false;

        /// <summary>
        /// Foreign key to the project (optional)
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Navigation property to the project
        /// </summary>
        public virtual Project? Project { get; set; }
    }
} 