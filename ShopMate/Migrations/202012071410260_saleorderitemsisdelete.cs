namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleorderitemsisdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrderItems", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrderItems", "IsDeleted");
        }
    }
}
