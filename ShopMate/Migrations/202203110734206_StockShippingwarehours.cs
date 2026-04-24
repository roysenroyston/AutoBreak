namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockShippingwarehours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockShippingOrders", "Warehouse", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockShippingOrders", "Warehouse");
        }
    }
}
