namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currencycontroller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotations", "CurrencyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quotations", "CurrencyId");
        }
    }
}
