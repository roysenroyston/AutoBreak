namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remarksinvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceMaterials", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceMaterials", "Remarks");
        }
    }
}
