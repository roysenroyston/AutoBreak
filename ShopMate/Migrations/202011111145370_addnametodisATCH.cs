namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnametodisATCH : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DispatchMaterials", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DispatchMaterials", "Name");
        }
    }
}
