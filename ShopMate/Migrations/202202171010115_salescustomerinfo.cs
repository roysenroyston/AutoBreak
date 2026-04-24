namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salescustomerinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "CustomerInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "CustomerInfo");
        }
    }
}
