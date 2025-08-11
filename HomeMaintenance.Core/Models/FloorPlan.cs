using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a floor plan for the home
    /// </summary>
    public class FloorPlan : BaseEntity
    {
        /// <summary>
        /// Name or title of the floor plan
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the floor plan
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Floor level (e.g., Basement, Ground, First, Second)
        /// </summary>
        [StringLength(50)]
        public string? FloorLevel { get; set; }

        /// <summary>
        /// SVG data for rendering the floor plan
        /// </summary>
        public string? SvgData { get; set; }

        /// <summary>
        /// Image URL or path for the floor plan
        /// </summary>
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Width of the floor plan in pixels or units
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Height of the floor plan in pixels or units
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Scale factor for the floor plan (e.g., 1 inch = 10 feet)
        /// </summary>
        public string? Scale { get; set; }

        /// <summary>
        /// Whether this is the default floor plan
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Order of the floor plan for display
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Notes about the floor plan
        /// </summary>
        public string? Notes { get; set; }
    }
} 