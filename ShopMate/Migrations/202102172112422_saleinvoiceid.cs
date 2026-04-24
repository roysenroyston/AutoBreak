namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleinvoiceid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "InvoiceId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "InvoiceId");
        }
    }
}
