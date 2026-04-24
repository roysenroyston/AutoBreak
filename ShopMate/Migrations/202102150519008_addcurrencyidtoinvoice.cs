namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcurrencyidtoinvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformalInvoices", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "CurrencyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "CurrencyId");
            DropColumn("dbo.InformalInvoices", "CurrencyId");
        }
    }
}
