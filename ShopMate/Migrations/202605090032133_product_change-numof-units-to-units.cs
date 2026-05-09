namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_changenumofunitstounits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Units", c => c.Int(nullable: false));
            DropColumn("dbo.Product", "NumOfUnitCase");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "NumOfUnitCase", c => c.Int(nullable: false));
            DropColumn("dbo.Product", "Units");
        }
    }
}
