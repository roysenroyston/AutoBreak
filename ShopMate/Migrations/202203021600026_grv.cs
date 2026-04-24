namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grv : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GRVMaterials", name: "GRV_Id", newName: "GRVId");
            RenameIndex(table: "dbo.GRVMaterials", name: "IX_GRV_Id", newName: "IX_GRVId");
            AddColumn("dbo.GRVMaterials", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.GRVMaterials", "Status", c => c.String(nullable: false));
            AddColumn("dbo.GRVMaterials", "GRV_GRVId_Id", c => c.Int());
            AddColumn("dbo.GRVs", "Warehouse", c => c.Int(nullable: false));
            CreateIndex("dbo.GRVMaterials", "ProductId");
            CreateIndex("dbo.GRVMaterials", "GRV_GRVId_Id");
            AddForeignKey("dbo.GRVMaterials", "GRV_GRVId_Id", "dbo.GRVs", "Id");
            AddForeignKey("dbo.GRVMaterials", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GRVMaterials", "ProductId", "dbo.Product");
            DropForeignKey("dbo.GRVMaterials", "GRV_GRVId_Id", "dbo.GRVs");
            DropIndex("dbo.GRVMaterials", new[] { "GRV_GRVId_Id" });
            DropIndex("dbo.GRVMaterials", new[] { "ProductId" });
            DropColumn("dbo.GRVs", "Warehouse");
            DropColumn("dbo.GRVMaterials", "GRV_GRVId_Id");
            DropColumn("dbo.GRVMaterials", "Status");
            DropColumn("dbo.GRVMaterials", "ProductId");
            RenameIndex(table: "dbo.GRVMaterials", name: "IX_GRVId", newName: "IX_GRV_Id");
            RenameColumn(table: "dbo.GRVMaterials", name: "GRVId", newName: "GRV_Id");
        }
    }
}
