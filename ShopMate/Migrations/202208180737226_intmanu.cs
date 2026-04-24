namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intmanu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FinishedGoods", "manufacturingId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FinishedGoods", "manufacturingId", c => c.Int(nullable: false));
        }
    }
}
