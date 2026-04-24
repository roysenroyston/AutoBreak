namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exenseaddinvatnumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "InvoiceNumber", c => c.String());
            AddColumn("dbo.Expense", "TaxAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Expense", "VatNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expense", "VatNumber");
            DropColumn("dbo.Expense", "TaxAmount");
            DropColumn("dbo.Expense", "InvoiceNumber");
        }
    }
}
