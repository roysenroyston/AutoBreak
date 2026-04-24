namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unittaxqut : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuotationItems", "unitTax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuotationItems", "unitTax");
        }
    }
}
