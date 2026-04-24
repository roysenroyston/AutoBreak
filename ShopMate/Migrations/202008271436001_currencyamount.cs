namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currencyamount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "CurrencyAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "CurrencyAmount");
        }
    }
}
