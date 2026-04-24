namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "PurchasePrice", c => c.String());
            DropColumn("dbo.RawMaterials", "CostPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RawMaterials", "CostPrice", c => c.String());
            DropColumn("dbo.RawMaterials", "PurchasePrice");
        }
    }
}
