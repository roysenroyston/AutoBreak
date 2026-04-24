namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quotations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuotationItems", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.QuotationItems", "TaxId", c => c.Int());
            AddColumn("dbo.QuotationItems", "Name", c => c.String());
            AddColumn("dbo.Quotations", "WarehouseId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuotationItems", "ProductId");
            AddForeignKey("dbo.QuotationItems", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuotationItems", "ProductId", "dbo.Product");
            DropIndex("dbo.QuotationItems", new[] { "ProductId" });
            DropColumn("dbo.Quotations", "WarehouseId");
            DropColumn("dbo.QuotationItems", "Name");
            DropColumn("dbo.QuotationItems", "TaxId");
            DropColumn("dbo.QuotationItems", "ProductId");
        }
    }
}
