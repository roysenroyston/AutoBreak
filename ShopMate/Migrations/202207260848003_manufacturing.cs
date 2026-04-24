namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manufacturing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturings", "FinishedGoodsId", c => c.Int(nullable: false));
            AddColumn("dbo.Manufacturings", "CutSheet", c => c.String());
            AddColumn("dbo.Manufacturings", "Remarks", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturings", "Remarks");
            DropColumn("dbo.Manufacturings", "CutSheet");
            DropColumn("dbo.Manufacturings", "FinishedGoodsId");
        }
    }
}
