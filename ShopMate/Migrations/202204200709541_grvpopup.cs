namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grvpopup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GRVs", "StockShippingOrderId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GRVs", "StockShippingOrderId");
        }
    }
}
