namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispatchat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformalInvoices", "DispatchAt", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InformalInvoices", "DispatchAt");
        }
    }
}
