namespace ShopMate.ModelDto
{
	public class AppProductViewDto
	{
		public int id { get; set; }
		public string name { get; set; }
		public decimal price { get; set; }
		public string image { get; set; }
		public decimal tax { get; set; }
		public string barcode { get; set; }
		public decimal quantity { get; set; }
		public int productType { get; set; }
		public int numOfSinglesInCase { get; set; }
		public decimal remainingSinglesQuantity { get; set; }
		public decimal remainingQuantity { get; set; } // Same as quantity in your LINQ
		public decimal unitSalePrice { get; set; }
		public decimal? parentRemainingQuantity { get; set; } // Extra field for parent stock
	}
}