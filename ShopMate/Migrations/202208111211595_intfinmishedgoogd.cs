namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intfinmishedgoogd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FinishedGoods", "FinishedQty", c => c.Int(nullable: false));
            AlterColumn("dbo.finishedItems", "Qty", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.finishedItems", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FinishedGoods", "FinishedQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
