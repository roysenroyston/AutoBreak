namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rawmaterials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "BarCode", c => c.String(maxLength: 100));
            AddColumn("dbo.RawMaterials", "ProductCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterials", "ProductCategory");
            DropColumn("dbo.RawMaterials", "BarCode");
        }
    }
}
