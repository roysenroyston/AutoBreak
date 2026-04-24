namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Manufacturings", "InventoryTypeId", "dbo.InventoryType");
            DropIndex("dbo.Manufacturings", new[] { "InventoryTypeId" });
            AlterColumn("dbo.Manufacturings", "InventoryTypeId", c => c.Int());
            CreateIndex("dbo.Manufacturings", "InventoryTypeId");
            AddForeignKey("dbo.Manufacturings", "InventoryTypeId", "dbo.InventoryType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Manufacturings", "InventoryTypeId", "dbo.InventoryType");
            DropIndex("dbo.Manufacturings", new[] { "InventoryTypeId" });
            AlterColumn("dbo.Manufacturings", "InventoryTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Manufacturings", "InventoryTypeId");
            AddForeignKey("dbo.Manufacturings", "InventoryTypeId", "dbo.InventoryType", "Id", cascadeDelete: true);
        }
    }
}
