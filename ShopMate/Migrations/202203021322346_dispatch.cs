namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WarehouseStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        WarehouseId = c.Int(nullable: false),
                        RemainingQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Warehouse", t => t.WarehouseId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.WarehouseId);
            
            AddColumn("dbo.InvoiceItems", "RemainingQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Dispatches", "CustomerUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "IsDispatched", c => c.Boolean(nullable: false));
            AddColumn("dbo.Invoice", "DispatchAt", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseStocks", "WarehouseId", "dbo.Warehouse");
            DropForeignKey("dbo.WarehouseStocks", "ProductId", "dbo.Product");
            DropIndex("dbo.WarehouseStocks", new[] { "WarehouseId" });
            DropIndex("dbo.WarehouseStocks", new[] { "ProductId" });
            DropColumn("dbo.Invoice", "DispatchAt");
            DropColumn("dbo.Invoice", "IsDispatched");
            DropColumn("dbo.Dispatches", "CustomerUserId");
            DropColumn("dbo.InvoiceItems", "RemainingQuantity");
            DropTable("dbo.WarehouseStocks");
        }
    }
}
