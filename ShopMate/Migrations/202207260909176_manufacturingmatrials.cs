namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manufacturingmatrials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManufacturingMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinishedGoodsId = c.Int(nullable: false),
                        CutSheet = c.String(),
                        Remarks = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Manufacturings", "FinishedGoodsId");
            DropColumn("dbo.Manufacturings", "CutSheet");
            DropColumn("dbo.Manufacturings", "Remarks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Manufacturings", "Remarks", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Manufacturings", "CutSheet", c => c.String());
            AddColumn("dbo.Manufacturings", "FinishedGoodsId", c => c.Int(nullable: false));
            DropTable("dbo.ManufacturingMaterials");
        }
    }
}
