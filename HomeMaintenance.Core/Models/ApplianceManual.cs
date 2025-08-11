using System;
using System.ComponentModel.DataAnnotations;

namespace HomeMaintenance.Core.Models
{
    /// <summary>
    /// Represents a manual or document associated with an appliance
    /// </summary>
    public class ApplianceManual : BaseEntity
    {
        /// <summary>
        /// Title or name of the manual
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the manual
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// File name of the manual
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File path or URL where the manual is stored
        /// </summary>
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// MIME type of the file
        /// </summary>
        [StringLength(100)]
        public string? ContentType { get; set; }

        /// <summary>
        /// Version of the manual
        /// </summary>
        [StringLength(50)]
        public string? Version { get; set; }

        /// <summary>
        /// Date when the manual was uploaded
        /// </summary>
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Foreign key to the associated appliance
        /// </summary>
        public int ApplianceId { get; set; }

        /// <summary>
        /// Navigation property to the associated appliance
        /// </summary>
        public virtual Appliance Appliance { get; set; } = null!;
    }
} 