namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_mainparentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "MainParentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "MainParentId");
        }
    }
}
