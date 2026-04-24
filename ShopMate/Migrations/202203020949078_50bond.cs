namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _50bond : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclaredayEnds", "fiftybond", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeclaredayEnds", "fiftybond");
        }
    }
}
