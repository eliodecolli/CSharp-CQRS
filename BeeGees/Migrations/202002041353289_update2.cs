namespace BeeGees_WriteNode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Write_Shipments", "DeliveredAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Write_Shipments", "DeliveredAt", c => c.DateTime(nullable: false));
        }
    }
}
