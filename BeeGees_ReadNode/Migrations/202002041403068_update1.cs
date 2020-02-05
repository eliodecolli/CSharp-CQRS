namespace BeeGees_ReadNode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Read_Shipments",
                c => new
                    {
                        ShipmentId = c.Guid(nullable: false),
                        ShipmentName = c.String(),
                        CustomerId = c.String(),
                        Location = c.String(),
                        Status = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ShipmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Read_Shipments");
        }
    }
}
