namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newyu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GRVs", "receivedby", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GRVs", "receivedby", c => c.String(nullable: false));
        }
    }
}
