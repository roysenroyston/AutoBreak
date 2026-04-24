namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datestring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "PaymentDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "PaymentDate", c => c.DateTime(nullable: false));
        }
    }
}
