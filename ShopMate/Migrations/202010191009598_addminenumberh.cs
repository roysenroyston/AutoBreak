namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addminenumberh : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "MinePermitNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "MinePermitNumber", c => c.String());
        }
    }
}
