namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tomaiAriel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "BarCode", c => c.String(maxLength: 100));
            AddColumn("dbo.RawMaterials", "ProductCategory", c => c.String());
            AddColumn("dbo.Stores", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.Stores", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stores", "RemainingQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stores", "ProductCategory", c => c.String());
            AddColumn("dbo.Stores", "BarCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "BarCode");
            DropColumn("dbo.Stores", "ProductCategory");
            DropColumn("dbo.Stores", "RemainingQuantity");
            DropColumn("dbo.Stores", "PurchasePrice");
            DropColumn("dbo.Stores", "Name");
            DropColumn("dbo.RawMaterials", "ProductCategory");
            DropColumn("dbo.RawMaterials", "BarCode");
        }
    }
}
