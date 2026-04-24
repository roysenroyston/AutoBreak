namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreturnedquantitytosales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "ReturnQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "ReturnQuantity");
        }
    }
}
