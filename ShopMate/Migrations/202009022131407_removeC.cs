namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeC : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.finishedItems", "description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.finishedItems", "description", c => c.String(nullable: false));
        }
    }
}
