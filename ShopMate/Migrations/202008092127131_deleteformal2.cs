namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteformal2 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.ProductStock", "IsFormal2");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.ProductStock", "IsFormal2", c => c.Boolean(nullable: false));
        }
    }
}
