using System;
namespace OnlineStore.Models
{
	public class ProductWithProperPriceDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public long OriginalPrice { get; set; }
		public double PriceAfterDiscount { get; set; }
		public bool HasDiscount { get; set; }
		public double Discount { get; set; }
		public int InventoryCount { get; set; }
	}
}

