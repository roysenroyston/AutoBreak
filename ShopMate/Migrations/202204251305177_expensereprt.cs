namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expensereprt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "LedgerAccount_LedgerAccountId_Id", c => c.Int());
            CreateIndex("dbo.Expense", "LedgerAccount_LedgerAccountId_Id");
            AddForeignKey("dbo.Expense", "LedgerAccount_LedgerAccountId_Id", "dbo.LedgerAccount", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expense", "LedgerAccount_LedgerAccountId_Id", "dbo.LedgerAccount");
            DropIndex("dbo.Expense", new[] { "LedgerAccount_LedgerAccountId_Id" });
            DropColumn("dbo.Expense", "LedgerAccount_LedgerAccountId_Id");
        }
    }
}
