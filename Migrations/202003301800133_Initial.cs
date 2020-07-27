namespace LogBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        LocationId = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        Onsite = c.Boolean(nullable: false),
                        Comment = c.String(nullable: false),
                        User = c.String(nullable: false),
                        LocationType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LocationTypes", t => t.LocationType_Id, cascadeDelete: true)
                .Index(t => new { t.Timestamp, t.LocationId }, name: "IX_DateLocation")
                .Index(t => t.LocationType_Id);
            
            CreateTable(
                "dbo.ReasonForResponses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Abbreviation = c.String(nullable: false, maxLength: 10),
                        Group = c.String(maxLength: 10),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReasonForResponseLogs",
                c => new
                    {
                        ReasonForResponse_Id = c.Int(nullable: false),
                        Log_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReasonForResponse_Id, t.Log_Id })
                .ForeignKey("dbo.ReasonForResponses", t => t.ReasonForResponse_Id, cascadeDelete: true)
                .ForeignKey("dbo.Logs", t => t.Log_Id, cascadeDelete: true)
                .Index(t => t.ReasonForResponse_Id)
                .Index(t => t.Log_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReasonForResponseLogs", "Log_Id", "dbo.Logs");
            DropForeignKey("dbo.ReasonForResponseLogs", "ReasonForResponse_Id", "dbo.ReasonForResponses");
            DropForeignKey("dbo.Logs", "LocationType_Id", "dbo.LocationTypes");
            DropIndex("dbo.ReasonForResponseLogs", new[] { "Log_Id" });
            DropIndex("dbo.ReasonForResponseLogs", new[] { "ReasonForResponse_Id" });
            DropIndex("dbo.Logs", new[] { "LocationType_Id" });
            DropIndex("dbo.Logs", "IX_DateLocation");
            DropTable("dbo.ReasonForResponseLogs");
            DropTable("dbo.ReasonForResponses");
            DropTable("dbo.Logs");
            DropTable("dbo.LocationTypes");
        }
    }
}
