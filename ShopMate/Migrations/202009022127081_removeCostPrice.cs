namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeCostPrice : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FinishedGoods", "CostPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FinishedGoods", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
