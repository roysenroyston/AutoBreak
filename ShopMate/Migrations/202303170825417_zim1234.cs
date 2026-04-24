namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zim1234 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclaredayEnds", "hundredbond", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeclaredayEnds", "hundredbond");
        }
    }
}
