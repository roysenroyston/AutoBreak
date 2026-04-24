namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "QuotationId", c => c.Int());
            CreateIndex("dbo.SaleOrders", "QuotationId");
            AddForeignKey("dbo.SaleOrders", "QuotationId", "dbo.Quotations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "QuotationId", "dbo.Quotations");
            DropIndex("dbo.SaleOrders", new[] { "QuotationId" });
            DropColumn("dbo.SaleOrders", "QuotationId");
        }
    }
}
