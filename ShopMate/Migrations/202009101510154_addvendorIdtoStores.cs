namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvendorIdtoStores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "VendorUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Stores", "User_VendorUserId_Id", c => c.Int());
            CreateIndex("dbo.Stores", "User_VendorUserId_Id");
            AddForeignKey("dbo.Stores", "User_VendorUserId_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "User_VendorUserId_Id", "dbo.User");
            DropIndex("dbo.Stores", new[] { "User_VendorUserId_Id" });
            DropColumn("dbo.Stores", "User_VendorUserId_Id");
            DropColumn("dbo.Stores", "VendorUserId");
        }
    }
}
