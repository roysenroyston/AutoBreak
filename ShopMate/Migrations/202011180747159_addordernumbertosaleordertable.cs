namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addordernumbertosaleordertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "CustomerOrderNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "CustomerOrderNumber");
        }
    }
}
