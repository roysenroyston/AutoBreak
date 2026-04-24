namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkingupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expense", "User_VendorUserId_Id", c => c.Int());
            CreateIndex("dbo.Expense", "User_VendorUserId_Id");
            AddForeignKey("dbo.Expense", "User_VendorUserId_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expense", "User_VendorUserId_Id", "dbo.User");
            DropIndex("dbo.Expense", new[] { "User_VendorUserId_Id" });
            DropColumn("dbo.Expense", "User_VendorUserId_Id");
        }
    }
}
