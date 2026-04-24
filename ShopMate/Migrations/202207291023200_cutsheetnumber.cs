namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cutsheetnumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturings", "CutSheet", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturings", "CutSheet");
        }
    }
}
