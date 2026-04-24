namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class disatchmaterial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DispatchMaterials", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.DispatchMaterials", "ProductId");
            AddForeignKey("dbo.DispatchMaterials", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DispatchMaterials", "ProductId", "dbo.Product");
            DropIndex("dbo.DispatchMaterials", new[] { "ProductId" });
            DropColumn("dbo.DispatchMaterials", "ProductId");
        }
    }
}
