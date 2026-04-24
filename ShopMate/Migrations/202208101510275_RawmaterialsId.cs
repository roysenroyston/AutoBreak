namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RawmaterialsId : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Stores", "RawMaterials_Id", "dbo.RawMaterials");
            //DropIndex("dbo.Stores", new[] { "RawMaterials_Id" });
            //RenameColumn(table: "dbo.Stores", name: "RawMaterials_Id", newName: "RawMaterialsId");
            //AlterColumn("dbo.Stores", "RawMaterialsId", c => c.Int(nullable: false));
            //CreateIndex("dbo.Stores", "RawMaterialsId");
            //AddForeignKey("dbo.Stores", "RawMaterialsId", "dbo.RawMaterials", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "RawMaterialsId", "dbo.RawMaterials");
            DropIndex("dbo.Stores", new[] { "RawMaterialsId" });
            AlterColumn("dbo.Stores", "RawMaterialsId", c => c.Int());
            RenameColumn(table: "dbo.Stores", name: "RawMaterialsId", newName: "RawMaterials_Id");
            CreateIndex("dbo.Stores", "RawMaterials_Id");
            AddForeignKey("dbo.Stores", "RawMaterials_Id", "dbo.RawMaterials", "Id");
        }
    }
}
