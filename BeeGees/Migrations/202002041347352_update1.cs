namespace BeeGees_WriteNode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Write_Shipments",
                c => new
                    {
                        ShipmentId = c.Guid(nullable: false),
                        ShipmentAddress = c.String(),
                        ShipmentName = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        DeliveredAt = c.DateTime(nullable: false),
                        CurrentStatus = c.String(),
                        CurrentLocation = c.String(),
                        CustomerId = c.String(),
                    })
                .PrimaryKey(t => t.ShipmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Write_Shipments");
        }
    }
}
