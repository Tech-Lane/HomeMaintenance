using HomeMaintenance.Core.Models;
using HomeMaintenance.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeMaintenance.API;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HomeMaintenanceDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Appliances.AnyAsync())
        {
            return; // Data already seeded
        }

        // Seed Appliances
        var appliances = new List<Appliance>
        {
            new Appliance
            {
                Name = "Kitchen Refrigerator",
                Brand = "Samsung",
                Model = "RF28T5000SR",
                Category = "Kitchen",
                Location = "Kitchen",
                SerialNumber = "SN123456789",
                PurchaseDate = DateTime.Now.AddYears(-2),
                PurchasePrice = 1299.99m,
                WarrantyExpirationDate = DateTime.Now.AddMonths(6),
                ManualUrl = "https://www.samsung.com/us/support/manuals/RF28T5000SR",
                Notes = "French door refrigerator with ice maker",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Appliance
            {
                Name = "Washing Machine",
                Brand = "LG",
                Model = "WT7300CW",
                Category = "Laundry",
                Location = "Laundry Room",
                SerialNumber = "SN987654321",
                PurchaseDate = DateTime.Now.AddYears(-1),
                PurchasePrice = 899.99m,
                WarrantyExpirationDate = DateTime.Now.AddMonths(3),
                ManualUrl = "https://www.lg.com/us/support/manuals/WT7300CW",
                Notes = "Front load washer with steam function",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Appliance
            {
                Name = "Dishwasher",
                Brand = "Bosch",
                Model = "SHEM63W55N",
                Category = "Kitchen",
                Location = "Kitchen",
                SerialNumber = "SN456789123",
                PurchaseDate = DateTime.Now.AddMonths(-6),
                PurchasePrice = 649.99m,
                WarrantyExpirationDate = DateTime.Now.AddYears(1),
                ManualUrl = "https://www.bosch-home.com/us/support/manuals/SHEM63W55N",
                Notes = "Quiet dishwasher with third rack",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Appliance
            {
                Name = "HVAC System",
                Brand = "Carrier",
                Model = "24ACC6",
                Category = "HVAC",
                Location = "Basement",
                SerialNumber = "SN789123456",
                PurchaseDate = DateTime.Now.AddYears(-3),
                PurchasePrice = 3500.00m,
                WarrantyExpirationDate = DateTime.Now.AddMonths(-2), // Expired
                ManualUrl = "https://www.carrier.com/us/support/manuals/24ACC6",
                Notes = "Central air conditioning unit",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Appliance
            {
                Name = "Microwave",
                Brand = "Panasonic",
                Model = "NN-SN966S",
                Category = "Kitchen",
                Location = "Kitchen",
                SerialNumber = "SN321654987",
                PurchaseDate = DateTime.Now.AddMonths(-3),
                PurchasePrice = 199.99m,
                WarrantyExpirationDate = DateTime.Now.AddMonths(9),
                ManualUrl = "https://www.panasonic.com/us/support/manuals/NN-SN966S",
                Notes = "Inverter microwave with sensor cooking",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.Appliances.AddRangeAsync(appliances);
        await context.SaveChangesAsync();

        // Seed Maintenance Tasks
        var maintenanceTasks = new List<MaintenanceTask>
        {
            new MaintenanceTask
            {
                Title = "Clean Refrigerator Coils",
                Description = "Clean the condenser coils to improve efficiency",
                ApplianceId = appliances[0].Id, // Refrigerator
                Frequency = "Yearly",
                FrequencyInterval = 1,
                NextDueDate = DateTime.Now.AddDays(30),
                Priority = "Medium",
                EstimatedTimeMinutes = 30,
                Instructions = "Unplug refrigerator, remove back panel, vacuum coils",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new MaintenanceTask
            {
                Title = "Replace Washing Machine Hoses",
                Description = "Replace water supply hoses to prevent leaks",
                ApplianceId = appliances[1].Id, // Washing Machine
                Frequency = "Every 5 Years",
                FrequencyInterval = 5,
                NextDueDate = DateTime.Now.AddDays(-10), // Overdue
                Priority = "High",
                EstimatedTimeMinutes = 60,
                Instructions = "Turn off water, disconnect hoses, install new ones",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new MaintenanceTask
            {
                Title = "Clean Dishwasher Filter",
                Description = "Remove and clean the dishwasher filter",
                ApplianceId = appliances[2].Id, // Dishwasher
                Frequency = "Monthly",
                FrequencyInterval = 1,
                NextDueDate = DateTime.Now.AddDays(7),
                Priority = "Low",
                EstimatedTimeMinutes = 15,
                Instructions = "Remove bottom rack, unscrew filter, clean with brush",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new MaintenanceTask
            {
                Title = "HVAC Filter Replacement",
                Description = "Replace the air filter for better air quality",
                ApplianceId = appliances[3].Id, // HVAC
                Frequency = "Monthly",
                FrequencyInterval = 1,
                NextDueDate = DateTime.Now.AddDays(-5), // Overdue
                Priority = "High",
                EstimatedTimeMinutes = 10,
                Instructions = "Turn off system, remove old filter, install new one",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.MaintenanceTasks.AddRangeAsync(maintenanceTasks);
        await context.SaveChangesAsync();

        // Seed Chores
        var chores = new List<Chore>
        {
            new Chore
            {
                Title = "Clean Kitchen Counters",
                Description = "Wipe down all kitchen countertops",
                Category = "Kitchen",
                Frequency = "Daily",
                FrequencyInterval = 1,
                DueDate = DateTime.Now.Date,
                Priority = "Medium",
                AssignedTo = "Family",
                EstimatedTimeMinutes = 15,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Chore
            {
                Title = "Vacuum Living Room",
                Description = "Vacuum the living room carpet and furniture",
                Category = "Living Room",
                Frequency = "Weekly",
                FrequencyInterval = 1,
                DueDate = DateTime.Now.AddDays(2),
                Priority = "Low",
                AssignedTo = "John",
                EstimatedTimeMinutes = 20,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Chore
            {
                Title = "Clean Bathroom",
                Description = "Clean toilet, sink, and shower",
                Category = "Bathroom",
                Frequency = "Weekly",
                FrequencyInterval = 1,
                DueDate = DateTime.Now.AddDays(-1), // Overdue
                Priority = "High",
                AssignedTo = "Jane",
                EstimatedTimeMinutes = 30,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.Chores.AddRangeAsync(chores);
        await context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully!");
    }
} 