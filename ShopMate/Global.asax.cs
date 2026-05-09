using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ShopMate.Models;
using TrackerEnabledDbContext.Common.Configuration;

namespace ShopMate
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
			GlobalTrackingConfig.DisconnectedContext = true;
			//EnsureIndexes();

		}


		private void EnsureIndexes()
		{
			using (var ctx = new SIContext())
			{
				using (var transaction = ctx.Database.BeginTransaction())
				{
					// Index 1
					ctx.Database.ExecuteSqlCommand(@"
            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_IsActive_Name_Covering')
            CREATE NONCLUSTERED INDEX IX_Products_IsActive_Name_Covering
            ON Product (IsActive, Name)
            INCLUDE (Id, MainParentId, TaxId, SalePrice, ProductImage, BarCode, ProductType, NumOfSinglesInCase, UnitSalePrice);
        ");

					// Index 2
					ctx.Database.ExecuteSqlCommand(@"
            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseStocks_WarehouseId_RemainingQuantity_Covering')
            CREATE NONCLUSTERED INDEX IX_WarehouseStocks_WarehouseId_RemainingQuantity_Covering
            ON WarehouseStocks (WarehouseId, RemainingQuantity)
            INCLUDE (ProductId, Id, RemainingSinglesQuantity);
        ");

					transaction.Commit();
				}
			}
		}
	}
}

