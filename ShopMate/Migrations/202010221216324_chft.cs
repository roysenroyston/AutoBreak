namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chft : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expense", "ExpenseId", "dbo.Expense");
            DropIndex("dbo.Expense", new[] { "ExpenseId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Expense", "ExpenseId");
            AddForeignKey("dbo.Expense", "ExpenseId", "dbo.Expense", "Id");
        }
    }
}
