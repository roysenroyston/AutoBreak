namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Isremoved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturings", "IsRemoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturings", "IsRemoved");
        }
    }
}
