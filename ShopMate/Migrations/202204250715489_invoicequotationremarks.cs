namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoicequotationremarks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformalInvoices", "Remarks", c => c.String());
            AddColumn("dbo.Invoice", "Remarks", c => c.String());
            AddColumn("dbo.Quotations", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotations", "Remarks");
            DropColumn("dbo.Invoice", "Remarks");
            DropColumn("dbo.InformalInvoices", "Remarks");
        }
    }
}
