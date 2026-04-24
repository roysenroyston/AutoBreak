namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Expense", "Vendorname", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Expense", "Vendorname", c => c.String());
        }
    }
}
