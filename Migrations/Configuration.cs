using System.Collections.Generic;
using LogBook.Models.LogBook;

namespace LogBook.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LogBook.Models.LogBook.LogBook>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LogBook.Models.LogBook.LogBook context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var signalLocation = new LocationType { Description = "Signal" };
            var rwisLocation = new LocationType { Description = "RWIS" };
            context.LocationTypes.AddOrUpdate(l => l.Description, signalLocation, rwisLocation);
            var complaint = new ReasonForResponse
            {
                Id = 1,
                Description = "Complaint/Work Order",
                DisplayOrder = 1,
                Abbreviation = "CW"
            };
            var pmRm = new ReasonForResponse
            {
                Id = 2,
                Description = "Preventative Maintenance",
                DisplayOrder = 2,
                Abbreviation = "PM",
                Group = "General"
            };
            var afterHours = new ReasonForResponse
            {
                Id = 3,
                Description = "On-call/After Hours",
                DisplayOrder = 3,
                Abbreviation = "EMR",
                Group = "General"
            };
            var maintenanceEquipment = new ReasonForResponse
            {
                Id = 4,
                Description = "Failed Equipment",
                DisplayOrder = 4,
                Abbreviation = "E",
                Group = "WO"
            };
            var maintenanceDetection = new ReasonForResponse
            {
                Id = 5,
                Description = "Detection Related",
                DisplayOrder = 5,
                Abbreviation = "D",
                Group = "WO"
            };
            var timing = new ReasonForResponse
            {
                Id = 6,
                Description = "Timing",
                DisplayOrder = 6,
                Abbreviation = "T",
                Group = "WO"
            };
            var other = new ReasonForResponse
            {
                Id = 7,
                Description = "Other",
                DisplayOrder = 7,
                Abbreviation = "O",
                Group = "General"
            }; ;
            context.ReasonForResponses.AddOrUpdate(
                r => r.Id,
                complaint,
                pmRm,
                afterHours,
                other,
                maintenanceEquipment,
                maintenanceDetection,
                timing
        );

            context.SaveChanges();
            context.Logs.AddOrUpdate(new Log
            {
                User = "dlowe",
                LocationId = "7220",
                ReasonForResponses = new List<ReasonForResponse> { timing, afterHours },
                LocationType = signalLocation,
                Timestamp = DateTime.Now,
                Comment = "This is a test comment.",
                Onsite = true,
                DateCreated = DateTime.Now
            });
        }
    
    }
}
