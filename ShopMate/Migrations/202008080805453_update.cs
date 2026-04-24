namespace ShopMate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Accountpayments",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.Int(nullable: false),
            //            Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Remarks = c.String(nullable: false, maxLength: 200),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            WarehouseId = c.Int(nullable: false),
            //            IsReturn = c.Boolean(nullable: false),
            //            cash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            swipe = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ecocash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Change = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.User",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserName = c.String(nullable: false, maxLength: 100),
            //            Password = c.String(nullable: false, maxLength: 100),
            //            FullName = c.String(maxLength: 111),
            //            Mobile = c.String(maxLength: 15),
            //            Address = c.String(maxLength: 200),
            //            About = c.String(maxLength: 500),
            //            RoleId = c.Int(nullable: false),
            //            JoinDate = c.DateTime(),
            //            IsActive = c.Boolean(),
            //            CanOrder = c.Boolean(),
            //            CanLogin = c.Boolean(nullable: false),
            //            vatNumber = c.String(maxLength: 150),
            //            WarehouseId = c.Int(),
            //            credit = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Role", t => t.RoleId)
            //    .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.DuePayment",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            UserId = c.Int(nullable: false),
            //            DueAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Remarks = c.String(nullable: false, maxLength: 200),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            WarehouseId = c.Int(nullable: false),
            //            IsReturn = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.UserId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.MenuPermission",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            MenuId = c.Int(),
            //            RoleId = c.Int(nullable: false),
            //            UserId = c.Int(),
            //            SortOrder = c.Int(),
            //            IsCreate = c.Boolean(nullable: false),
            //            IsRead = c.Boolean(nullable: false),
            //            IsUpdate = c.Boolean(nullable: false),
            //            IsDelete = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Menu", t => t.MenuId)
            //    .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
            //    .ForeignKey("dbo.User", t => t.UserId)
            //    .Index(t => t.MenuId)
            //    .Index(t => t.RoleId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.Menu",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            MenuText = c.String(nullable: false, maxLength: 100),
            //            MenuURL = c.String(nullable: false, maxLength: 400),
            //            ParentId = c.Int(),
            //            SortOrder = c.Int(),
            //            MenuIcon = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Menu", t => t.ParentId)
            //    .Index(t => t.ParentId);
            
            //CreateTable(
            //    "dbo.Role",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            RoleName = c.String(nullable: false, maxLength: 50),
            //            IsActive = c.Boolean(nullable: false),
            //            WarehouseId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Purchase",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            VendorUserId = c.Int(nullable: false),
            //            ProductId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmountWithTax = c.Decimal(precision: 18, scale: 2),
            //            DateAdded = c.DateTime(nullable: false),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            InventoryTypeId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.ProductId)
            //    .ForeignKey("dbo.User", t => t.VendorUserId)
            //    .Index(t => t.VendorUserId)
            //    .Index(t => t.ProductId);
            
            //CreateTable(
            //    "dbo.Product",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            ProductCategoryId = c.Int(nullable: false),
            //            BarCode = c.String(maxLength: 100),
            //            PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ProductImage = c.String(maxLength: 100),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            DateModied = c.DateTime(),
            //            IsActive = c.Boolean(nullable: false),
            //            StockAlert = c.Int(nullable: false),
            //            TaxId = c.Int(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //            Discount = c.Decimal(precision: 18, scale: 2),
            //            RemainingQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            HSN = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId)
            //    .Index(t => t.ProductCategoryId);
            
            //CreateTable(
            //    "dbo.InvoiceItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductId = c.Int(nullable: false),
            //            InvoiceId = c.Int(),
            //            InformalInvoiceId = c.Int(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmountWithTax = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            TaxId = c.Int(),
            //            PurchaseId = c.Int(),
            //            SaleId = c.Int(),
            //            ProductStockId = c.Int(nullable: false),
            //            TransactionId = c.Int(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.ProductId)
            //    .ForeignKey("dbo.InformalInvoices", t => t.InformalInvoiceId)
            //    .ForeignKey("dbo.Invoice", t => t.InvoiceId)
            //    .Index(t => t.ProductId)
            //    .Index(t => t.InvoiceId)
            //    .Index(t => t.InformalInvoiceId);
            
            //CreateTable(
            //    "dbo.ProductCategory",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            ParentId = c.Int(),
            //            WarehouseId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.ProductCategory", t => t.ParentId)
            //    .Index(t => t.ParentId);
            
            //CreateTable(
            //    "dbo.ProductStock",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalPurchaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalSaleAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalSaleAmountWithTax = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Discount = c.Decimal(precision: 18, scale: 2),
            //            CGST = c.Int(),
            //            CGST_Rate = c.Decimal(precision: 18, scale: 2),
            //            SGST = c.Int(),
            //            SGST_Rate = c.Decimal(precision: 18, scale: 2),
            //            IGST = c.Int(),
            //            IGST_Rate = c.Decimal(precision: 18, scale: 2),
            //            TaxId = c.Int(),
            //            OtherTaxValue = c.Decimal(precision: 18, scale: 2),
            //            Description = c.String(maxLength: 200),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            DateModied = c.DateTime(),
            //            InventoryTypeId = c.Int(nullable: false),
            //            SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Profit = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ProfitWithTax = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId)
            //    .ForeignKey("dbo.Product", t => t.ProductId)
            //    .Index(t => t.ProductId)
            //    .Index(t => t.InventoryTypeId);
            
            //CreateTable(
            //    "dbo.InventoryType",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Sale",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CustomerUserId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PaymentModeId = c.Int(nullable: false),
            //            TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmountWithTax = c.Decimal(precision: 18, scale: 2),
            //            PaidAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ProductId = c.Int(nullable: false),
            //            DateAdded = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            DateModied = c.DateTime(),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            InventoryTypeId = c.Int(),
            //            ecocash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            telecash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            onemoney = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            rtgs = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            usd = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            bond = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            fca = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            isFormalSale = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.PaymentMode", t => t.PaymentModeId)
            //    .ForeignKey("dbo.Product", t => t.ProductId)
            //    .ForeignKey("dbo.User", t => t.CustomerUserId)
            //    .Index(t => t.CustomerUserId)
            //    .Index(t => t.PaymentModeId)
            //    .Index(t => t.ProductId);
            
            //CreateTable(
            //    "dbo.PaymentMode",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AuditLogs",
            //    c => new
            //        {
            //            AuditLogId = c.Long(nullable: false, identity: true),
            //            UserName = c.String(),
            //            EventDateUTC = c.DateTime(nullable: false),
            //            EventType = c.Int(nullable: false),
            //            TypeFullName = c.String(nullable: false, maxLength: 512),
            //            RecordId = c.String(nullable: false, maxLength: 256),
            //            Discriminator = c.String(nullable: false, maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.AuditLogId);
            
            //CreateTable(
            //    "dbo.AuditLogDetails",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            PropertyName = c.String(nullable: false, maxLength: 256),
            //            OriginalValue = c.String(),
            //            NewValue = c.String(),
            //            AuditLogId = c.Long(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
            //    .Index(t => t.AuditLogId);
            
            //CreateTable(
            //    "dbo.LogMetadata",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            AuditLogId = c.Long(nullable: false),
            //            Key = c.String(),
            //            Value = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
            //    .Index(t => t.AuditLogId);
            
            //CreateTable(
            //    "dbo.AuditLoggers",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Username = c.String(),
            //            Event = c.Int(nullable: false),
            //            TimeOfEvent = c.DateTime(nullable: false),
            //            Details = c.String(),
            //            RecordId = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
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
            //    "dbo.Currencies",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.DeclaredayEnds",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Declared = c.Boolean(nullable: false),
            //            UserId = c.Int(nullable: false),
            //            fivecents = c.Int(),
            //            tencents = c.Int(),
            //            twentyfivecents = c.Int(),
            //            fiftycents = c.Int(),
            //            onedollars = c.Int(),
            //            twodollars = c.Int(),
            //            fiveDollars = c.Int(),
            //            tenDollars = c.Int(),
            //            tdollars = c.Int(),
            //            fiftyDollars = c.Int(),
            //            hundreddollars = c.Int(),
            //            totalcash = c.Decimal(precision: 18, scale: 2),
            //            usd = c.Decimal(precision: 18, scale: 2),
            //            totalecocash = c.Decimal(precision: 18, scale: 2),
            //            telecash = c.Decimal(precision: 18, scale: 2),
            //            onemoney = c.Decimal(precision: 18, scale: 2),
            //            rtgs = c.Decimal(precision: 18, scale: 2),
            //            nostro = c.Decimal(precision: 18, scale: 2),
            //            totalAmount = c.Decimal(precision: 18, scale: 2),
            //            totalChange = c.Decimal(precision: 18, scale: 2),
            //            totalTelecash = c.Decimal(precision: 18, scale: 2),
            //            totalOnemoney = c.Decimal(precision: 18, scale: 2),
            //            totalCashUsd = c.Decimal(precision: 18, scale: 2),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            DateModied = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.DeliveryNotes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            invoiceNo = c.Int(nullable: false),
            //            OrderNo = c.Int(nullable: false),
            //            CustomerUserId = c.Int(nullable: false),
            //            delivered = c.Boolean(nullable: false),
            //            ddate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Dispatches",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            DispatchTo = c.String(),
            //            invoiceNo = c.Int(nullable: false),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            DateAdded = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.DispatchMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DispatchId = c.Int(nullable: false),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            DateAdded = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Dispatches", t => t.DispatchId, cascadeDelete: true)
            //    .Index(t => t.DispatchId);
            
            //CreateTable(
            //    "dbo.DNoteMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DNoteId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.DeliveryNotes", t => t.DNoteId, cascadeDelete: true)
            //    .Index(t => t.DNoteId);
            
            //CreateTable(
            //    "dbo.Expense",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Remarks = c.String(nullable: false, maxLength: 200),
            //            Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.FinishedGoods",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            WarehouseId = c.Int(nullable: false),
            //            finisheddate = c.DateTime(),
            //            AddedBy = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.finishedItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            description = c.String(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            unitprice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Total = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            InventoryTypeId = c.Int(),
            //            ProductId = c.Int(nullable: false),
            //            ProductStockId = c.Int(nullable: false),
            //            TransactionId = c.Int(nullable: false),
            //            dateadded = c.DateTime(nullable: false),
            //            WarehouseId = c.Int(),
            //            finishedgoods_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.FinishedGoods", t => t.finishedgoods_Id)
            //    .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId)
            //    .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
            //    .ForeignKey("dbo.Warehouse", t => t.WarehouseId)
            //    .Index(t => t.InventoryTypeId)
            //    .Index(t => t.ProductId)
            //    .Index(t => t.WarehouseId)
            //    .Index(t => t.finishedgoods_Id);
            
            //CreateTable(
            //    "dbo.Warehouse",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            Address = c.String(maxLength: 300),
            //            Mobile = c.String(maxLength: 15),
            //            Email = c.String(maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.GRVMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Description = c.String(nullable: false),
            //            UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            GRV_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.GRVs", t => t.GRV_Id)
            //    .Index(t => t.GRV_Id);
            
            //CreateTable(
            //    "dbo.GRVs",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            supplier = c.String(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Description = c.String(),
            //            receivedby = c.String(nullable: false),
            //            OrderNumber = c.Int(nullable: false),
            //            UnitPrice = c.Decimal(precision: 18, scale: 2),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            purchasedate = c.DateTime(),
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
            //    "dbo.InvoiceMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            description = c.String(),
            //            quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            rate = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            vat = c.String(),
            //            InvoiceId = c.Int(),
            //            InformalInvoiceId = c.Int(),
            //            ProductId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
            //    .ForeignKey("dbo.InformalInvoices", t => t.InformalInvoiceId)
            //    .ForeignKey("dbo.Invoice", t => t.InvoiceId)
            //    .Index(t => t.InvoiceId)
            //    .Index(t => t.InformalInvoiceId)
            //    .Index(t => t.ProductId);
            
            //CreateTable(
            //    "dbo.InvoiceFormat",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CompanyName = c.String(nullable: false, maxLength: 100),
            //            Logo = c.String(maxLength: 100),
            //            AddressInfo = c.String(maxLength: 500),
            //            OtherInfo = c.String(maxLength: 500),
            //            FooterInfo = c.String(maxLength: 500),
            //            WarehouseId = c.Int(),
            //            VatNumber = c.String(maxLength: 100),
            //            BPNumber = c.String(maxLength: 100),
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
            //    "dbo.Invoice",
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
            //            IsPurchaseOrSale = c.String(maxLength: 20),
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
            //    "dbo.InvoiceTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.JobCardMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            material = c.String(),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            price = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            JobCard_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.JobCards", t => t.JobCard_Id)
            //    .Index(t => t.JobCard_Id);
            
            //CreateTable(
            //    "dbo.JobCards",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Description = c.String(),
            //            OrderNumber = c.Int(nullable: false),
            //            JobNo = c.Int(nullable: false),
            //            sandries = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            totalbfvat = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalAmountWithTax = c.Decimal(precision: 18, scale: 2),
            //            customername = c.Int(),
            //            address = c.String(),
            //            purchasedate = c.DateTime(),
            //            completed = c.Boolean(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //            customername_customernameId_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.customername_customernameId_Id)
            //    .Index(t => t.customername_customernameId_Id);
            
            //CreateTable(
            //    "dbo.JobCardServices",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            machineused = c.Int(),
            //            artisan = c.Int(),
            //            hours = c.String(),
            //            rate = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            InvoiceId = c.Int(),
            //            JobCardId = c.Int(),
            //            Machine_MachineId_Id = c.Int(),
            //            User_UserId_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Invoice", t => t.InvoiceId)
            //    .ForeignKey("dbo.JobCards", t => t.JobCardId)
            //    .ForeignKey("dbo.Machines", t => t.Machine_MachineId_Id)
            //    .ForeignKey("dbo.User", t => t.User_UserId_Id)
            //    .Index(t => t.InvoiceId)
            //    .Index(t => t.JobCardId)
            //    .Index(t => t.Machine_MachineId_Id)
            //    .Index(t => t.User_UserId_Id);
            
            //CreateTable(
            //    "dbo.Machines",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //            Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            rate = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            depreciation = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.LedgerAccount",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            ParentId = c.Int(),
            //            DateAdded = c.DateTime(),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.LedgerAccount", t => t.ParentId)
            //    .Index(t => t.ParentId);
            
            //CreateTable(
            //    "dbo.Transaction",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            DebitLedgerAccountId = c.Int(),
            //            DebitAmount = c.Decimal(precision: 18, scale: 2),
            //            CreditLedgerAccountId = c.Int(),
            //            CreditAmount = c.Decimal(precision: 18, scale: 2),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            Other = c.String(maxLength: 100),
            //            PurchaseOrSale = c.String(maxLength: 20),
            //            PurchaseIdOrSaleId = c.Int(),
            //            Remarks = c.String(maxLength: 100),
            //            WarehouseId = c.Int(nullable: false),
            //            IsFormal = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.LedgerAccount", t => t.CreditLedgerAccountId)
            //    .ForeignKey("dbo.LedgerAccount", t => t.DebitLedgerAccountId)
            //    .Index(t => t.DebitLedgerAccountId)
            //    .Index(t => t.CreditLedgerAccountId);
            
            //CreateTable(
            //    "dbo.Manufacturings",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            RawMaterialsId = c.Int(nullable: false),
            //            DateAdded = c.DateTime(),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            InventoryTypeId = c.Int(nullable: false),
            //            Remaining = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId, cascadeDelete: true)
            //    .ForeignKey("dbo.RawMaterials", t => t.RawMaterialsId, cascadeDelete: true)
            //    .Index(t => t.RawMaterialsId)
            //    .Index(t => t.InventoryTypeId);
            
            //CreateTable(
            //    "dbo.RawMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            StockAlert = c.Int(nullable: false),
            //            TaxId = c.Int(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //            RemainingQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.RawMaterialStocks",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            RawMaterialsId = c.Int(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalPurchaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Description = c.String(),
            //            AddedBy = c.Int(),
            //            DateAdded = c.DateTime(),
            //            InventoryTypeId = c.Int(nullable: false),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId, cascadeDelete: true)
            //    .ForeignKey("dbo.RawMaterials", t => t.RawMaterialsId, cascadeDelete: true)
            //    .Index(t => t.RawMaterialsId)
            //    .Index(t => t.InventoryTypeId);
            
            //CreateTable(
            //    "dbo.Stores",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            totalprice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            WarehouseId = c.Int(nullable: false),
            //            purchasedate = c.DateTime(),
            //            AddedBy = c.Int(),
            //            RawMaterials_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.RawMaterials", t => t.RawMaterials_Id)
            //    .Index(t => t.RawMaterials_Id);
            
            //CreateTable(
            //    "dbo.OrderMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Description = c.String(),
            //            OrderId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
            //    .Index(t => t.OrderId);
            
            //CreateTable(
            //    "dbo.Orders",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            goods = c.String(nullable: false),
            //            supplier = c.Int(nullable: false),
            //            orderNumber = c.Int(),
            //            purchasedate = c.DateTime(),
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
            //    "dbo.Paymenttracks",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            cash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            swipe = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            ecocash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            onemoney = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            telecash = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            usd = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Rand = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            pula = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Change = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            SaleId = c.Int(),
            //            InvoiceId = c.Int(),
            //            DateAdded = c.DateTime(),
            //            ModifiedBy = c.Int(),
            //            DateModied = c.DateTime(),
            //            AddedBy = c.Int(),
            //            WarehouseId = c.Int(nullable: false),
            //            AccountpaymentId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Invoice", t => t.InvoiceId)
            //    .ForeignKey("dbo.Sale", t => t.SaleId)
            //    .Index(t => t.SaleId)
            //    .Index(t => t.InvoiceId);
            
            //CreateTable(
            //    "dbo.PaymentTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.QuotationItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Description = c.String(),
            //            UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            QuotationId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Quotations", t => t.QuotationId, cascadeDelete: true)
            //    .Index(t => t.QuotationId);
            
            //CreateTable(
            //    "dbo.Quotations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Total = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AddedBy = c.Int(nullable: false),
            //            customerId = c.Int(nullable: false),
            //            IssueDate = c.DateTime(nullable: false),
            //            ValidUntil = c.DateTime(),
            //            ModifiedBy = c.Int(nullable: false),
            //            approved = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.customerId, cascadeDelete: true)
            //    .Index(t => t.customerId);
            
            //CreateTable(
            //    "dbo.Rates",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CurrencyRate = c.Double(nullable: false),
            //            DateModified = c.DateTime(),
            //            CurrencyId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Currencies", t => t.CurrencyId, cascadeDelete: true)
            //    .Index(t => t.CurrencyId);
            
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
            
            //CreateTable(
            //    "dbo.Setting",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            sKey = c.String(nullable: false, maxLength: 200),
            //            sValue = c.String(nullable: false, maxLength: 3000),
            //            sGroup = c.String(nullable: false, maxLength: 500),
            //            WarehouseId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.StockTakeDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductId = c.Int(nullable: false),
            //            actualquantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            counted = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            actualvalue = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            countedvalue = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            variance = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            variancevalue = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            WarehouseId = c.Int(nullable: false),
            //            StockTakeId = c.Int(nullable: false),
            //            Product_ProductId_Id = c.Int(),
            //            StockTake_StockTakeId_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.Product_ProductId_Id)
            //    .ForeignKey("dbo.Product", t => t.StockTake_StockTakeId_Id)
            //    .Index(t => t.Product_ProductId_Id)
            //    .Index(t => t.StockTake_StockTakeId_Id);
            
            //CreateTable(
            //    "dbo.StockTakes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ProductCategoryId = c.Int(),
            //            ProductId = c.Int(nullable: false),
            //            actualquantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            counted = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            addedby = c.Int(nullable: false),
            //            variancevalue = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateAdded = c.DateTime(),
            //            WarehouseId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
            //    .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId)
            //    .Index(t => t.ProductCategoryId)
            //    .Index(t => t.ProductId);
            
            //CreateTable(
            //    "dbo.StoresMaterials",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            goods = c.String(nullable: false),
            //            Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            unitprice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Total = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            InventoryTypeId = c.Int(),
            //            RawMaterialsId = c.Int(nullable: false),
            //            rawmaterialStockId = c.Int(nullable: false),
            //            TransactionId = c.Int(nullable: false),
            //            store_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.InventoryType", t => t.InventoryTypeId)
            //    .ForeignKey("dbo.Stores", t => t.store_Id)
            //    .Index(t => t.InventoryTypeId)
            //    .Index(t => t.store_Id);
            
            //CreateTable(
            //    "dbo.Tax",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Other = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoresMaterials", "store_Id", "dbo.Stores");
            DropForeignKey("dbo.StoresMaterials", "InventoryTypeId", "dbo.InventoryType");
            DropForeignKey("dbo.StockTakes", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.StockTakes", "ProductId", "dbo.Product");
            DropForeignKey("dbo.StockTakeDetails", "StockTake_StockTakeId_Id", "dbo.Product");
            DropForeignKey("dbo.StockTakeDetails", "Product_ProductId_Id", "dbo.Product");
            DropForeignKey("dbo.SaleOrders", "User_CustomerId_Id", "dbo.User");
            DropForeignKey("dbo.SaleOrderItems", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Rates", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.QuotationItems", "QuotationId", "dbo.Quotations");
            DropForeignKey("dbo.Quotations", "customerId", "dbo.User");
            DropForeignKey("dbo.Paymenttracks", "SaleId", "dbo.Sale");
            DropForeignKey("dbo.Paymenttracks", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.Payments", "User_UserId_Id", "dbo.User");
            DropForeignKey("dbo.Payments", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Payments", "BankId", "dbo.Banks");
            DropForeignKey("dbo.OrderMaterials", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Stores", "RawMaterials_Id", "dbo.RawMaterials");
            DropForeignKey("dbo.RawMaterialStocks", "RawMaterialsId", "dbo.RawMaterials");
            DropForeignKey("dbo.RawMaterialStocks", "InventoryTypeId", "dbo.InventoryType");
            DropForeignKey("dbo.Manufacturings", "RawMaterialsId", "dbo.RawMaterials");
            DropForeignKey("dbo.Manufacturings", "InventoryTypeId", "dbo.InventoryType");
            DropForeignKey("dbo.Transaction", "DebitLedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Transaction", "CreditLedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccount", "ParentId", "dbo.LedgerAccount");
            DropForeignKey("dbo.JobCardServices", "User_UserId_Id", "dbo.User");
            DropForeignKey("dbo.JobCardServices", "Machine_MachineId_Id", "dbo.Machines");
            DropForeignKey("dbo.JobCardServices", "JobCardId", "dbo.JobCards");
            DropForeignKey("dbo.JobCardServices", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.JobCardMaterials", "JobCard_Id", "dbo.JobCards");
            DropForeignKey("dbo.JobCards", "customername_customernameId_Id", "dbo.User");
            DropForeignKey("dbo.InvoiceMaterials", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.InvoiceMaterials", "InformalInvoiceId", "dbo.InformalInvoices");
            DropForeignKey("dbo.InvoiceMaterials", "ProductId", "dbo.Product");
            DropForeignKey("dbo.InvoiceItems", "InformalInvoiceId", "dbo.InformalInvoices");
            DropForeignKey("dbo.GRVMaterials", "GRV_Id", "dbo.GRVs");
            DropForeignKey("dbo.finishedItems", "WarehouseId", "dbo.Warehouse");
            DropForeignKey("dbo.finishedItems", "ProductId", "dbo.Product");
            DropForeignKey("dbo.finishedItems", "InventoryTypeId", "dbo.InventoryType");
            DropForeignKey("dbo.finishedItems", "finishedgoods_Id", "dbo.FinishedGoods");
            DropForeignKey("dbo.DNoteMaterials", "DNoteId", "dbo.DeliveryNotes");
            DropForeignKey("dbo.DispatchMaterials", "DispatchId", "dbo.Dispatches");
            DropForeignKey("dbo.LogMetadata", "AuditLogId", "dbo.AuditLogs");
            DropForeignKey("dbo.AuditLogDetails", "AuditLogId", "dbo.AuditLogs");
            DropForeignKey("dbo.Accountpayments", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Purchase", "VendorUserId", "dbo.User");
            DropForeignKey("dbo.Purchase", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Sale", "CustomerUserId", "dbo.User");
            DropForeignKey("dbo.Sale", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Sale", "PaymentModeId", "dbo.PaymentMode");
            DropForeignKey("dbo.ProductStock", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductStock", "InventoryTypeId", "dbo.InventoryType");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.ProductCategory", "ParentId", "dbo.ProductCategory");
            DropForeignKey("dbo.InvoiceItems", "ProductId", "dbo.Product");
            DropForeignKey("dbo.MenuPermission", "UserId", "dbo.User");
            DropForeignKey("dbo.MenuPermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.MenuPermission", "MenuId", "dbo.Menu");
            DropForeignKey("dbo.Menu", "ParentId", "dbo.Menu");
            DropForeignKey("dbo.DuePayment", "UserId", "dbo.User");
            DropIndex("dbo.StoresMaterials", new[] { "store_Id" });
            DropIndex("dbo.StoresMaterials", new[] { "InventoryTypeId" });
            DropIndex("dbo.StockTakes", new[] { "ProductId" });
            DropIndex("dbo.StockTakes", new[] { "ProductCategoryId" });
            DropIndex("dbo.StockTakeDetails", new[] { "StockTake_StockTakeId_Id" });
            DropIndex("dbo.StockTakeDetails", new[] { "Product_ProductId_Id" });
            DropIndex("dbo.SaleOrders", new[] { "User_CustomerId_Id" });
            DropIndex("dbo.SaleOrderItems", new[] { "ProductId" });
            DropIndex("dbo.Rates", new[] { "CurrencyId" });
            DropIndex("dbo.Quotations", new[] { "customerId" });
            DropIndex("dbo.QuotationItems", new[] { "QuotationId" });
            DropIndex("dbo.Paymenttracks", new[] { "InvoiceId" });
            DropIndex("dbo.Paymenttracks", new[] { "SaleId" });
            DropIndex("dbo.Payments", new[] { "User_UserId_Id" });
            DropIndex("dbo.Payments", new[] { "CurrencyId" });
            DropIndex("dbo.Payments", new[] { "BankId" });
            DropIndex("dbo.OrderMaterials", new[] { "OrderId" });
            DropIndex("dbo.Stores", new[] { "RawMaterials_Id" });
            DropIndex("dbo.RawMaterialStocks", new[] { "InventoryTypeId" });
            DropIndex("dbo.RawMaterialStocks", new[] { "RawMaterialsId" });
            DropIndex("dbo.Manufacturings", new[] { "InventoryTypeId" });
            DropIndex("dbo.Manufacturings", new[] { "RawMaterialsId" });
            DropIndex("dbo.Transaction", new[] { "CreditLedgerAccountId" });
            DropIndex("dbo.Transaction", new[] { "DebitLedgerAccountId" });
            DropIndex("dbo.LedgerAccount", new[] { "ParentId" });
            DropIndex("dbo.JobCardServices", new[] { "User_UserId_Id" });
            DropIndex("dbo.JobCardServices", new[] { "Machine_MachineId_Id" });
            DropIndex("dbo.JobCardServices", new[] { "JobCardId" });
            DropIndex("dbo.JobCardServices", new[] { "InvoiceId" });
            DropIndex("dbo.JobCards", new[] { "customername_customernameId_Id" });
            DropIndex("dbo.JobCardMaterials", new[] { "JobCard_Id" });
            DropIndex("dbo.InvoiceMaterials", new[] { "ProductId" });
            DropIndex("dbo.InvoiceMaterials", new[] { "InformalInvoiceId" });
            DropIndex("dbo.InvoiceMaterials", new[] { "InvoiceId" });
            DropIndex("dbo.GRVMaterials", new[] { "GRV_Id" });
            DropIndex("dbo.finishedItems", new[] { "finishedgoods_Id" });
            DropIndex("dbo.finishedItems", new[] { "WarehouseId" });
            DropIndex("dbo.finishedItems", new[] { "ProductId" });
            DropIndex("dbo.finishedItems", new[] { "InventoryTypeId" });
            DropIndex("dbo.DNoteMaterials", new[] { "DNoteId" });
            DropIndex("dbo.DispatchMaterials", new[] { "DispatchId" });
            DropIndex("dbo.LogMetadata", new[] { "AuditLogId" });
            DropIndex("dbo.AuditLogDetails", new[] { "AuditLogId" });
            DropIndex("dbo.Sale", new[] { "ProductId" });
            DropIndex("dbo.Sale", new[] { "PaymentModeId" });
            DropIndex("dbo.Sale", new[] { "CustomerUserId" });
            DropIndex("dbo.ProductStock", new[] { "InventoryTypeId" });
            DropIndex("dbo.ProductStock", new[] { "ProductId" });
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            DropIndex("dbo.InvoiceItems", new[] { "InformalInvoiceId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceItems", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.Purchase", new[] { "ProductId" });
            DropIndex("dbo.Purchase", new[] { "VendorUserId" });
            DropIndex("dbo.Menu", new[] { "ParentId" });
            DropIndex("dbo.MenuPermission", new[] { "UserId" });
            DropIndex("dbo.MenuPermission", new[] { "RoleId" });
            DropIndex("dbo.MenuPermission", new[] { "MenuId" });
            DropIndex("dbo.DuePayment", new[] { "UserId" });
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.Accountpayments", new[] { "UserId" });
            DropTable("dbo.Tax");
            DropTable("dbo.StoresMaterials");
            DropTable("dbo.StockTakes");
            DropTable("dbo.StockTakeDetails");
            DropTable("dbo.Setting");
            DropTable("dbo.SaleOrders");
            DropTable("dbo.SaleOrderItems");
            DropTable("dbo.Rates");
            DropTable("dbo.Quotations");
            DropTable("dbo.QuotationItems");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Paymenttracks");
            DropTable("dbo.Payments");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderMaterials");
            DropTable("dbo.Stores");
            DropTable("dbo.RawMaterialStocks");
            DropTable("dbo.RawMaterials");
            DropTable("dbo.Manufacturings");
            DropTable("dbo.Transaction");
            DropTable("dbo.LedgerAccount");
            DropTable("dbo.Machines");
            DropTable("dbo.JobCardServices");
            DropTable("dbo.JobCards");
            DropTable("dbo.JobCardMaterials");
            DropTable("dbo.InvoiceTypes");
            DropTable("dbo.Invoice");
            DropTable("dbo.InvoicePaymentMethods");
            DropTable("dbo.InvoiceFormat");
            DropTable("dbo.InvoiceMaterials");
            DropTable("dbo.InformalInvoices");
            DropTable("dbo.GRVs");
            DropTable("dbo.GRVMaterials");
            DropTable("dbo.Warehouse");
            DropTable("dbo.finishedItems");
            DropTable("dbo.FinishedGoods");
            DropTable("dbo.Expense");
            DropTable("dbo.DNoteMaterials");
            DropTable("dbo.DispatchMaterials");
            DropTable("dbo.Dispatches");
            DropTable("dbo.DeliveryNotes");
            DropTable("dbo.DeclaredayEnds");
            DropTable("dbo.Currencies");
            DropTable("dbo.Banks");
            DropTable("dbo.AuditLoggers");
            DropTable("dbo.LogMetadata");
            DropTable("dbo.AuditLogDetails");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.PaymentMode");
            DropTable("dbo.Sale");
            DropTable("dbo.InventoryType");
            DropTable("dbo.ProductStock");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Product");
            DropTable("dbo.Purchase");
            DropTable("dbo.Role");
            DropTable("dbo.Menu");
            DropTable("dbo.MenuPermission");
            DropTable("dbo.DuePayment");
            DropTable("dbo.User");
            DropTable("dbo.Accountpayments");
        }
    }
}
