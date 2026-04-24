namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateaddedbypyament : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "DateAdded", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "DateAdded");
        }
    }
}
