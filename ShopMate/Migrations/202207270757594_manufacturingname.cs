namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manufacturingname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturings", "RawMaterialsname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturings", "RawMaterialsname");
        }
    }
}
