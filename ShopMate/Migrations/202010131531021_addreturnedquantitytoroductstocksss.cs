namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreturnedquantitytoroductstocksss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductStock", "ReturnedQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductStock", "ReturnedQuantity");
        }
    }
}
