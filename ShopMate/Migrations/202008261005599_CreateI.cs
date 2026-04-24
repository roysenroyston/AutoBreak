namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateI : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "BankAccount", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "BankAccount", c => c.Int());
        }
    }
}
