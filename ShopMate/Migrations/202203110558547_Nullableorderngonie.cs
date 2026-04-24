namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nullableorderngonie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GRVs", "OrderNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GRVs", "OrderNumber", c => c.Int(nullable: false));
        }
    }
}
