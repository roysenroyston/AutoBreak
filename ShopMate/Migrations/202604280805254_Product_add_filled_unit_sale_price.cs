namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_add_filled_unit_sale_price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "UnitSalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "UnitSalePrice");
        }
    }
}
