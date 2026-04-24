namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grvproductname3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GRVMaterials", "Product", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GRVMaterials", "Product");
        }
    }
}
