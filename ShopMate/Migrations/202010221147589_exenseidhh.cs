namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exenseidhh : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Expense", new[] { "ExpenseId" });
            AlterColumn("dbo.Expense", "ExpenseId", c => c.Int());
            CreateIndex("dbo.Expense", "ExpenseId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Expense", new[] { "ExpenseId" });
            AlterColumn("dbo.Expense", "ExpenseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Expense", "ExpenseId");
        }
    }
}
