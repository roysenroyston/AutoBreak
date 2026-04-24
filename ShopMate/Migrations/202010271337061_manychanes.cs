namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manychanes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterialStocks", "CGST", c => c.Int());
            AddColumn("dbo.RawMaterialStocks", "CGST_Rate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RawMaterialStocks", "SGST", c => c.Int());
            AddColumn("dbo.RawMaterialStocks", "SGST_Rate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RawMaterialStocks", "IGST", c => c.Int());
            AddColumn("dbo.RawMaterialStocks", "IGST_Rate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RawMaterialStocks", "TaxId", c => c.Int());
            AddColumn("dbo.RawMaterialStocks", "OtherTaxValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RawMaterialStocks", "TotalPurchaseAmountWithTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RawMaterialStocks", "TaxAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterialStocks", "TaxAmount");
            DropColumn("dbo.RawMaterialStocks", "TotalPurchaseAmountWithTax");
            DropColumn("dbo.RawMaterialStocks", "OtherTaxValue");
            DropColumn("dbo.RawMaterialStocks", "TaxId");
            DropColumn("dbo.RawMaterialStocks", "IGST_Rate");
            DropColumn("dbo.RawMaterialStocks", "IGST");
            DropColumn("dbo.RawMaterialStocks", "SGST_Rate");
            DropColumn("dbo.RawMaterialStocks", "SGST");
            DropColumn("dbo.RawMaterialStocks", "CGST_Rate");
            DropColumn("dbo.RawMaterialStocks", "CGST");
        }
    }
}
