namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productstock : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.InvoiceMaterials", "InvoiceId", "dbo.Invoice");
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceMaterials", new[] { "InvoiceId" });
            //CreateTable(
            //    "dbo.Banks",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            AccountNumber = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.InformalInvoices",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            IsBilled = c.Boolean(nullable: false),
            //            UserId = c.Int(nullable: false),
            //            InvoiceNo = c.Int(),
            //            ProjectNumber = c.Int(),
            //            orderNumber = c.Int(),
            //            total = c.Decimal(precision: 18, scale: 2),
            //            payment = c.Decimal(precision: 18, scale: 2),
            //            balance = c.Decimal(precision: 18, scale: 2),
            //            vat = c.Decimal(precision: 18, scale: 2),
            //            subtotal = c.Decimal(precision: 18, scale: 2),
            //            Duedate = c.DateTime(),
            //            CustomerVatReg = c.String(),
            //            IsPurchaseOrSale = c.String(),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            DateModied = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            salesrepId = c.Int(),
            //            DatePaid = c.DateTime(),
            //            InvoicePaymentMethodId = c.Int(),
            //            CustomerId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.InvoicePaymentMethods",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            DueIn = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Payments",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            AddedBy = c.Int(nullable: false),
            //            InvoiceId = c.Int(),
            //            PaymentTypeId = c.Int(nullable: false),
            //            BankId = c.Int(),
            //            CustomerId = c.Int(nullable: false),
            //            BankAccount = c.Int(),
            //            CurrencyId = c.Int(),
            //            PaymentModeId = c.Int(nullable: false),
            //            PaymentReference = c.String(nullable: false, maxLength: 100),
            //            Amount = c.Double(nullable: false),
            //            PaymentDate = c.DateTime(nullable: false),
            //            User_UserId_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Banks", t => t.BankId)
            //    .ForeignKey("dbo.Currencies", t => t.CurrencyId)
            //    .ForeignKey("dbo.User", t => t.User_UserId_Id)
            //    .Index(t => t.BankId)
            //    .Index(t => t.CurrencyId)
            //    .Index(t => t.User_UserId_Id);

            //CreateTable(
            //    "dbo.PaymentTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.SaleOrderItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmountWithTax = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateAdded = c.DateTime(),
            //            TaxId = c.Int(),
            //            SaleOrderId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
            //    .Index(t => t.ProductId);

            //CreateTable(
            //    "dbo.SaleOrders",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CustomerId = c.Int(nullable: false),
            //            DateAdded = c.DateTime(nullable: false),
            //            DateModified = c.DateTime(nullable: false),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            IsProcessed = c.Boolean(nullable: false),
            //            ModifiedBy = c.Int(nullable: false),
            //            User_CustomerId_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.User_CustomerId_Id)
            //    .Index(t => t.User_CustomerId_Id);

            //AddColumn("dbo.InvoiceItems", "InformalInvoiceId", c => c.Int());
            //AddColumn("dbo.Invoice", "DatePaid", c => c.DateTime());
            //AddColumn("dbo.Invoice", "InvoicePaymentMethodId", c => c.Int());
            //AddColumn("dbo.Invoice", "CustomerId", c => c.Int());
            //AddColumn("dbo.InvoiceMaterials", "InformalInvoiceId", c => c.Int());
            //AddColumn("dbo.ProductStock", "IsFormal", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Sale", "isFormalSale", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Transaction", "IsFormal", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.InvoiceItems", "InvoiceId", c => c.Int());
            //AlterColumn("dbo.Invoice", "orderNumber", c => c.Int());
            //AlterColumn("dbo.InvoiceMaterials", "InvoiceId", c => c.Int());
            //CreateIndex("dbo.InvoiceItems", "InvoiceId");
            //CreateIndex("dbo.InvoiceItems", "InformalInvoiceId");
            //CreateIndex("dbo.InvoiceMaterials", "InvoiceId");
            //CreateIndex("dbo.InvoiceMaterials", "InformalInvoiceId");
            //AddForeignKey("dbo.InvoiceItems", "InformalInvoiceId", "dbo.InformalInvoices", "Id");
            //AddForeignKey("dbo.InvoiceMaterials", "InformalInvoiceId", "dbo.InformalInvoices", "Id");
            //AddForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoice", "Id");
            //AddForeignKey("dbo.InvoiceMaterials", "InvoiceId", "dbo.Invoice", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceMaterials", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.SaleOrders", "User_CustomerId_Id", "dbo.User");
            DropForeignKey("dbo.SaleOrderItems", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Payments", "User_UserId_Id", "dbo.User");
            DropForeignKey("dbo.Payments", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Payments", "BankId", "dbo.Banks");
            DropForeignKey("dbo.InvoiceMaterials", "InformalInvoiceId", "dbo.InformalInvoices");
            DropForeignKey("dbo.InvoiceItems", "InformalInvoiceId", "dbo.InformalInvoices");
            DropIndex("dbo.SaleOrders", new[] { "User_CustomerId_Id" });
            DropIndex("dbo.SaleOrderItems", new[] { "ProductId" });
            DropIndex("dbo.Payments", new[] { "User_UserId_Id" });
            DropIndex("dbo.Payments", new[] { "CurrencyId" });
            DropIndex("dbo.Payments", new[] { "BankId" });
            DropIndex("dbo.InvoiceMaterials", new[] { "InformalInvoiceId" });
            DropIndex("dbo.InvoiceMaterials", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceItems", new[] { "InformalInvoiceId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            AlterColumn("dbo.InvoiceMaterials", "InvoiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.Invoice", "orderNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.InvoiceItems", "InvoiceId", c => c.Int(nullable: false));
            DropColumn("dbo.Transaction", "IsFormal");
            DropColumn("dbo.Sale", "isFormalSale");
            DropColumn("dbo.ProductStock", "IsFormal");
            DropColumn("dbo.InvoiceMaterials", "InformalInvoiceId");
            DropColumn("dbo.Invoice", "CustomerId");
            DropColumn("dbo.Invoice", "InvoicePaymentMethodId");
            DropColumn("dbo.Invoice", "DatePaid");
            DropColumn("dbo.InvoiceItems", "InformalInvoiceId");
            DropTable("dbo.SaleOrders");
            DropTable("dbo.SaleOrderItems");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Payments");
            DropTable("dbo.InvoicePaymentMethods");
            DropTable("dbo.InformalInvoices");
            DropTable("dbo.Banks");
            CreateIndex("dbo.InvoiceMaterials", "InvoiceId");
            CreateIndex("dbo.InvoiceItems", "InvoiceId");
            AddForeignKey("dbo.InvoiceMaterials", "InvoiceId", "dbo.Invoice", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoice", "Id", cascadeDelete: true);
        }
    }
}
