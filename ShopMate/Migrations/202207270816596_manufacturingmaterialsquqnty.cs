namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manufacturingmaterialsquqnty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManufacturingMaterials", "FinishedGoodsName", c => c.String());
            AddColumn("dbo.ManufacturingMaterials", "FinishedGoodsQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManufacturingMaterials", "FinishedGoodsQuantity");
            DropColumn("dbo.ManufacturingMaterials", "FinishedGoodsName");
        }
    }
}
