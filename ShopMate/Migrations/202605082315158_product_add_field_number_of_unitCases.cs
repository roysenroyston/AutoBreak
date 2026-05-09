namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_add_field_number_of_unitCases : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "NumOfUnitCase", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "NumOfUnitCase");
        }
    }
}
