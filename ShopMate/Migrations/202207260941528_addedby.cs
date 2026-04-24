namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedby : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManufacturingMaterials", "AddedBy", c => c.Int());
            AddColumn("dbo.ManufacturingMaterials", "DateAdded", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManufacturingMaterials", "DateAdded");
            DropColumn("dbo.ManufacturingMaterials", "AddedBy");
        }
    }
}
