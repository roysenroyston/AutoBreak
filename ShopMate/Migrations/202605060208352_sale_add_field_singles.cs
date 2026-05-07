namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sale_add_field_singles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "Singles", c => c.Long(nullable: false));
            AddColumn("dbo.Sale", "UnitSalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "UnitSalePrice");
            DropColumn("dbo.Sale", "Singles");
        }
    }
}
