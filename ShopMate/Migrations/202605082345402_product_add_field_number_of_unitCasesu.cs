namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_add_field_number_of_unitCasesu : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Product", "NumOfSinglesInCase", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "NumOfSinglesInCase", c => c.Int());
        }
    }
}
