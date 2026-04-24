namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoice", "CustomerInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "CustomerInfo");
        }
    }
}
