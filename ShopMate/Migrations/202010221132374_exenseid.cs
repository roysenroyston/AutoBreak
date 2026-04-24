namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exenseid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "ExpenseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Expense", "ExpenseId");
            AddForeignKey("dbo.Expense", "ExpenseId", "dbo.Expense", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expense", "ExpenseId", "dbo.Expense");
            DropIndex("dbo.Expense", new[] { "ExpenseId" });
            DropColumn("dbo.Expense", "ExpenseId");
        }
    }
}
