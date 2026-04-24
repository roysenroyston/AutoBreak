namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class strnaccount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Banks", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Banks", "AccountNumber", c => c.Int(nullable: false));
        }
    }
}
