namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grvnote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryNotes", "CollectedBy", c => c.String(nullable: false));
            AddColumn("dbo.DeliveryNotes", "CollectorId", c => c.String(nullable: false));
            AddColumn("dbo.DeliveryNotes", "CollectingVehicleRegNo", c => c.String(nullable: false));
            AddColumn("dbo.DNoteMaterials", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.InformalInvoices", "DNote", c => c.Boolean(nullable: false));
            AddColumn("dbo.Invoice", "DNote", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DeliveryNotes", "OrderNo", c => c.String(nullable: false));
            CreateIndex("dbo.DNoteMaterials", "ProductId");
            AddForeignKey("dbo.DNoteMaterials", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DNoteMaterials", "ProductId", "dbo.Product");
            DropIndex("dbo.DNoteMaterials", new[] { "ProductId" });
            AlterColumn("dbo.DeliveryNotes", "OrderNo", c => c.Int(nullable: false));
            DropColumn("dbo.Invoice", "DNote");
            DropColumn("dbo.InformalInvoices", "DNote");
            DropColumn("dbo.DNoteMaterials", "ProductId");
            DropColumn("dbo.DeliveryNotes", "CollectingVehicleRegNo");
            DropColumn("dbo.DeliveryNotes", "CollectorId");
            DropColumn("dbo.DeliveryNotes", "CollectedBy");
        }
    }
}
