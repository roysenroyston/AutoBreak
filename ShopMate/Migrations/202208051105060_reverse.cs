namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reverse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "CostPrice", c => c.String());
            DropColumn("dbo.RawMaterials", "PurchasePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RawMaterials", "PurchasePrice", c => c.String());
            DropColumn("dbo.RawMaterials", "CostPrice");
        }
    }
}
