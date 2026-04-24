namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcurrencysymboltocurrencies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currencies", "CurrencySymbol", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Currencies", "CurrencySymbol");
        }
    }
}
