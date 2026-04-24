namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manuId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinishedGoods", "manufacturingId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinishedGoods", "manufacturingId");
        }
    }
}
