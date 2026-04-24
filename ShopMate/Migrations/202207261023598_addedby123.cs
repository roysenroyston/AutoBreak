namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedby123 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturings", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Manufacturings", "ManufacturingMaterialId", c => c.Int());
            AddColumn("dbo.Manufacturings", "ManufacturingMaterial_ManufacturingMaterialId_Id", c => c.Int());
            AddColumn("dbo.ManufacturingMaterials", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Manufacturings", "ManufacturingMaterialId");
            CreateIndex("dbo.Manufacturings", "ManufacturingMaterial_ManufacturingMaterialId_Id");
            AddForeignKey("dbo.Manufacturings", "ManufacturingMaterialId", "dbo.ManufacturingMaterials", "Id");
            AddForeignKey("dbo.Manufacturings", "ManufacturingMaterial_ManufacturingMaterialId_Id", "dbo.ManufacturingMaterials", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Manufacturings", "ManufacturingMaterial_ManufacturingMaterialId_Id", "dbo.ManufacturingMaterials");
            DropForeignKey("dbo.Manufacturings", "ManufacturingMaterialId", "dbo.ManufacturingMaterials");
            DropIndex("dbo.Manufacturings", new[] { "ManufacturingMaterial_ManufacturingMaterialId_Id" });
            DropIndex("dbo.Manufacturings", new[] { "ManufacturingMaterialId" });
            DropColumn("dbo.ManufacturingMaterials", "TotalAmount");
            DropColumn("dbo.Manufacturings", "ManufacturingMaterial_ManufacturingMaterialId_Id");
            DropColumn("dbo.Manufacturings", "ManufacturingMaterialId");
            DropColumn("dbo.Manufacturings", "UnitPrice");
        }
    }
}
