namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addminenumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "MinePermitNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "MinePermitNumber");
        }
    }
}
