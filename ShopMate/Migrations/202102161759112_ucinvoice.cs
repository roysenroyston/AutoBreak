namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ucinvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoice", "currencytotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Invoice", "currencyvat", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Invoice", "currencysubtotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "currencysubtotal");
            DropColumn("dbo.Invoice", "currencyvat");
            DropColumn("dbo.Invoice", "currencytotal");
        }
    }
}
