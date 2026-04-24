namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockorder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockShippingOrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockShippingOrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.StockShippingOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseFrom = c.Int(nullable: false),
                        WarehouseTo = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        AddedBy = c.Int(),
                        WarehouseId = c.Int(nullable: false),
                        IsDispatched = c.Boolean(nullable: false),
                        IsReceived = c.Boolean(nullable: false),
                        ModifiedBy = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockShippingOrderItems", "ProductId", "dbo.Product");
            DropIndex("dbo.StockShippingOrderItems", new[] { "ProductId" });
            DropTable("dbo.StockShippingOrders");
            DropTable("dbo.StockShippingOrderItems");
        }
    }
}
