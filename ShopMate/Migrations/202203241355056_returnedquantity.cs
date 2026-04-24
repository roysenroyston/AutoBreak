namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnedquantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ReturnedQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Sale", "ReturnedQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Sale", "ReturnQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sale", "ReturnQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Sale", "ReturnedQuantity");
            DropColumn("dbo.Product", "ReturnedQuantity");
        }
    }
}
