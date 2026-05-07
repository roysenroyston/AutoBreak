namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_add_field_remaining_singles_quantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "RemainingSinglesQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "RemainingSinglesQuantity");
        }
    }
}
