namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Autobreak : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductCaseId", c => c.Int());
            AddColumn("dbo.Product", "NumOfSinglesInCase", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "NumOfSinglesInCase");
            DropColumn("dbo.Product", "ProductCaseId");
        }
    }
}
