namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cutsheetfinishedgoods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.finishedItems", "cutsheet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.finishedItems", "cutsheet");
        }
    }
}
