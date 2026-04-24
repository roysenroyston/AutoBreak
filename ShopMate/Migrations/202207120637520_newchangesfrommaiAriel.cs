namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newchangesfrommaiAriel : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.RawMaterials", "BarCode");
            //DropColumn("dbo.RawMaterials", "ProductCategory");
            //DropColumn("dbo.Stores", "Name");
            //DropColumn("dbo.Stores", "PurchasePrice");
            //DropColumn("dbo.Stores", "RemainingQuantity");
            //DropColumn("dbo.Stores", "ProductCategory");
            //DropColumn("dbo.Stores", "BarCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stores", "BarCode", c => c.String(maxLength: 100));
            AddColumn("dbo.Stores", "ProductCategory", c => c.String());
            AddColumn("dbo.Stores", "RemainingQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stores", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stores", "Name", c => c.String(maxLength: 100));
            AddColumn("dbo.RawMaterials", "ProductCategory", c => c.String());
            AddColumn("dbo.RawMaterials", "BarCode", c => c.String(maxLength: 100));
        }
    }
}
