namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "customerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "customerName");
        }
    }
}
