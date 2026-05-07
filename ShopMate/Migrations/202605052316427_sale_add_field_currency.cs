namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sale_add_field_currency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "Currency", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "Currency");
        }
    }
}
