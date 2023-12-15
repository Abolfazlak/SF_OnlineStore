using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DataProvide
{
	public class Product
	{
        public Product()
        {
            OrderList = new List<Order>();
        }

		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
        [Required]
        public int InventoryCount { get; set; }
        [Required]
        public long Price { get; set; }
        public double Discount { get; set; }

        public virtual List<Order> OrderList { get; set; }

    }
}

