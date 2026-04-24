namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "VendorUserId", c => c.Int());
            AddColumn("dbo.RawMaterials", "User_VendorUserId_Id", c => c.Int());
            CreateIndex("dbo.RawMaterials", "User_VendorUserId_Id");
            AddForeignKey("dbo.RawMaterials", "User_VendorUserId_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawMaterials", "User_VendorUserId_Id", "dbo.User");
            DropIndex("dbo.RawMaterials", new[] { "User_VendorUserId_Id" });
            DropColumn("dbo.RawMaterials", "User_VendorUserId_Id");
            DropColumn("dbo.RawMaterials", "VendorUserId");
        }
    }
}
