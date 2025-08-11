using Microsoft.EntityFrameworkCore;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.Data
{
    /// <summary>
    /// Entity Framework DbContext for the Home Maintenance application
    /// </summary>
    public class HomeMaintenanceDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the HomeMaintenanceDbContext
        /// </summary>
        /// <param name="options">The DbContext options</param>
        public HomeMaintenanceDbContext(DbContextOptions<HomeMaintenanceDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Appliances DbSet
        /// </summary>
        public DbSet<Appliance> Appliances { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ApplianceManuals DbSet
        /// </summary>
        public DbSet<ApplianceManual> ApplianceManuals { get; set; } = null!;

        /// <summary>
        /// Gets or sets the MaintenanceTasks DbSet
        /// </summary>
        public DbSet<MaintenanceTask> MaintenanceTasks { get; set; } = null!;

        /// <summary>
        /// Gets or sets the TaskCompletions DbSet
        /// </summary>
        public DbSet<TaskCompletion> TaskCompletions { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Chores DbSet
        /// </summary>
        public DbSet<Chore> Chores { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ChoreCompletions DbSet
        /// </summary>
        public DbSet<ChoreCompletion> ChoreCompletions { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Projects DbSet
        /// </summary>
        public DbSet<Project> Projects { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ProjectTasks DbSet
        /// </summary>
        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;

        /// <summary>
        /// Gets or sets the CostEstimates DbSet
        /// </summary>
        public DbSet<CostEstimate> CostEstimates { get; set; } = null!;

        /// <summary>
        /// Gets or sets the FloorPlans DbSet
        /// </summary>
        public DbSet<FloorPlan> FloorPlans { get; set; } = null!;

        /// <summary>
        /// Configures the model using Fluent API
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Appliance entity
            modelBuilder.Entity<Appliance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.ManualUrl).HasMaxLength(500);
                entity.Property(e => e.PhotoUrl).HasMaxLength(500);
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasMany(e => e.MaintenanceTasks)
                    .WithOne(e => e.Appliance)
                    .HasForeignKey(e => e.ApplianceId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Manuals)
                    .WithOne(e => e.Appliance)
                    .HasForeignKey(e => e.ApplianceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ApplianceManual entity
            modelBuilder.Entity<ApplianceManual>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.Version).HasMaxLength(50);
            });

            // Configure MaintenanceTask entity
            modelBuilder.Entity<MaintenanceTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Frequency).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.Instructions);
                entity.Property(e => e.EstimatedCost).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasMany(e => e.Completions)
                    .WithOne(e => e.MaintenanceTask)
                    .HasForeignKey(e => e.MaintenanceTaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure TaskCompletion entity
            modelBuilder.Entity<TaskCompletion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompletedAt).IsRequired();
                entity.Property(e => e.CompletedBy).HasMaxLength(100);
                entity.Property(e => e.PhotoUrls);
                entity.Property(e => e.ActualCost).HasColumnType("decimal(18,2)");
            });

            // Configure Chore entity
            modelBuilder.Entity<Chore>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Frequency).HasMaxLength(20);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.AssignedTo).HasMaxLength(100);

                // Relationships
                entity.HasMany(e => e.Completions)
                    .WithOne(e => e.Chore)
                    .HasForeignKey(e => e.ChoreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ChoreCompletion entity
            modelBuilder.Entity<ChoreCompletion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompletedAt).IsRequired();
                entity.Property(e => e.CompletedBy).HasMaxLength(100);
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.PhotoUrls);
                entity.Property(e => e.EstimatedBudget).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualCost).HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasMany(e => e.Tasks)
                    .WithOne(e => e.Project)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.CostEstimates)
                    .WithOne(e => e.Project)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ProjectTask entity
            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.Instructions);
                entity.Property(e => e.AssignedTo).HasMaxLength(100);
                entity.Property(e => e.Dependencies);
                entity.Property(e => e.EstimatedCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.EstimatedHours).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualHours).HasColumnType("decimal(18,2)");
            });

            // Configure CostEstimate entity
            modelBuilder.Entity<CostEstimate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.EstimatedAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).HasMaxLength(3);
                entity.Property(e => e.Source).HasMaxLength(100);
                entity.Property(e => e.ContactInfo).HasMaxLength(200);
            });

            // Configure FloorPlan entity
            modelBuilder.Entity<FloorPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FloorLevel).HasMaxLength(50);
                entity.Property(e => e.SvgData);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Scale).HasMaxLength(50);
            });
        }
    }
} 