namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleremainingquantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "RemainingQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "RemainingQuantity");
        }
    }
}
