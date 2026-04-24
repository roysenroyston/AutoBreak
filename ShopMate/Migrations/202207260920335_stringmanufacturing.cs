namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringmanufacturing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ManufacturingMaterials", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ManufacturingMaterials", "Remarks", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
