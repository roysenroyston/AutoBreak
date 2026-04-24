namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class producttype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ProductType");
        }
    }
}
