namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFloatCostpricerawmaterials : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RawMaterials", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RawMaterials", "CostPrice", c => c.Single(nullable: false));
        }
    }
}
