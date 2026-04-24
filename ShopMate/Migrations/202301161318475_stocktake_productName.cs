namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stocktake_productName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockTakeDetails", "productName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockTakeDetails", "productName");
        }
    }
}
