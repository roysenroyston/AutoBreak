namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class warehousereturnedstock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseStocks", "ReturnedQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseStocks", "ReturnedQuantity");
        }
    }
}
