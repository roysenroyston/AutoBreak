namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Isdeletd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockShippingOrders", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockShippingOrders", "IsDeleted");
        }
    }
}
