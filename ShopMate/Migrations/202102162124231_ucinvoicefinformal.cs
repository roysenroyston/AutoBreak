namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ucinvoicefinformal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformalInvoices", "currencytotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.InformalInvoices", "currencyvat", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.InformalInvoices", "currencysubtotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InformalInvoices", "currencysubtotal");
            DropColumn("dbo.InformalInvoices", "currencyvat");
            DropColumn("dbo.InformalInvoices", "currencytotal");
        }
    }
}
