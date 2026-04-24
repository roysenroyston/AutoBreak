namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoiceitemRemarks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "Remarks");
        }
    }
}
