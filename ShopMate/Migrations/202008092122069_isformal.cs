namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isformal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductStock", "IsFormal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
