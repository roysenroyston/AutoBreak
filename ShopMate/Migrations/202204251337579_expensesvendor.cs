namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expensesvendor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "Vendorname", c => c.String());
            AddColumn("dbo.Expense", "User_UserFullName_Id", c => c.Int());
            CreateIndex("dbo.Expense", "User_UserFullName_Id");
            AddForeignKey("dbo.Expense", "User_UserFullName_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expense", "User_UserFullName_Id", "dbo.User");
            DropIndex("dbo.Expense", new[] { "User_UserFullName_Id" });
            DropColumn("dbo.Expense", "User_UserFullName_Id");
            DropColumn("dbo.Expense", "Vendorname");
        }
    }
}
