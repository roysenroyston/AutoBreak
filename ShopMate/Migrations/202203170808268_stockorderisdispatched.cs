namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockorderisdispatched : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderMaterials", "IsDispatched", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderMaterials", "IsDispatched");
        }
    }
}
