namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class goods : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StoresMaterials", "goods");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoresMaterials", "goods", c => c.String(nullable: false));
        }
    }
}
