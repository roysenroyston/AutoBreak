namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sale_field_totwithtax_not_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sale", "TotalAmountWithTax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sale", "TotalAmountWithTax", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
