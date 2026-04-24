namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grvwarehouseidngoni : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GRVs", "WarehouseId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GRVs", "WarehouseId");
        }
    }
}
