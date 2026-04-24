namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class suppierremoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GRVs", "supplier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GRVs", "supplier", c => c.Int(nullable: false));
        }
    }
}
