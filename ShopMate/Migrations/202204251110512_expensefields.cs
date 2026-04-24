namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expensefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "SubTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Expense", "InvoiceDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expense", "InvoiceDate");
            DropColumn("dbo.Expense", "SubTotal");
        }
    }
}
