namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newq : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Stores", "RawMaterialsId", "dbo.RawMaterials");
            //DropIndex("dbo.Stores", new[] { "RawMaterialsId" });
            //RenameColumn(table: "dbo.Stores", name: "RawMaterialsId", newName: "RawMaterials_Id");
            //AlterColumn("dbo.Stores", "RawMaterials_Id", c => c.Int());
            //CreateIndex("dbo.Stores", "RawMaterials_Id");
            //AddForeignKey("dbo.Stores", "RawMaterials_Id", "dbo.RawMaterials", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stores", "RawMaterials_Id", "dbo.RawMaterials");
            DropIndex("dbo.Stores", new[] { "RawMaterials_Id" });
            AlterColumn("dbo.Stores", "RawMaterials_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Stores", name: "RawMaterials_Id", newName: "RawMaterialsId");
            CreateIndex("dbo.Stores", "RawMaterialsId");
            AddForeignKey("dbo.Stores", "RawMaterialsId", "dbo.RawMaterials", "Id", cascadeDelete: true);
        }
    }
}
