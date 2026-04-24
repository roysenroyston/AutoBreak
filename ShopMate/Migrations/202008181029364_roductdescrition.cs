namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roductdescrition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ProductDescription");
        }
    }
}
