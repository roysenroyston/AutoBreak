namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dayend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclaredayEnds", "twobond", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "fivebond", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "tenbond", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "twentybond", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "twentydollars", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "ecocash", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.DeclaredayEnds", "fivecents");
            DropColumn("dbo.DeclaredayEnds", "tencents");
            DropColumn("dbo.DeclaredayEnds", "twentyfivecents");
            DropColumn("dbo.DeclaredayEnds", "fiftycents");
            DropColumn("dbo.DeclaredayEnds", "tdollars");
            DropColumn("dbo.DeclaredayEnds", "usd");
            DropColumn("dbo.DeclaredayEnds", "totalecocash");
            DropColumn("dbo.DeclaredayEnds", "totalTelecash");
            DropColumn("dbo.DeclaredayEnds", "totalOnemoney");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclaredayEnds", "totalOnemoney", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DeclaredayEnds", "totalTelecash", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DeclaredayEnds", "totalecocash", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DeclaredayEnds", "usd", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.DeclaredayEnds", "tdollars", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "fiftycents", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "twentyfivecents", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "tencents", c => c.Int());
            AddColumn("dbo.DeclaredayEnds", "fivecents", c => c.Int());
            DropColumn("dbo.DeclaredayEnds", "ecocash");
            DropColumn("dbo.DeclaredayEnds", "twentydollars");
            DropColumn("dbo.DeclaredayEnds", "twentybond");
            DropColumn("dbo.DeclaredayEnds", "tenbond");
            DropColumn("dbo.DeclaredayEnds", "fivebond");
            DropColumn("dbo.DeclaredayEnds", "twobond");
        }
    }
}
