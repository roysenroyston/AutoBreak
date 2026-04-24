namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisdispatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformalInvoices", "IsDispatched", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InformalInvoices", "IsDispatched");
        }
    }
}
