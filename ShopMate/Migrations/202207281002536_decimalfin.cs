namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class decimalfin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinishedGoods", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinishedGoods", "Approved");
        }
    }
}
