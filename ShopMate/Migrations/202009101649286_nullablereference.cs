namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablereference : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "PaymentReference", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "PaymentReference", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
