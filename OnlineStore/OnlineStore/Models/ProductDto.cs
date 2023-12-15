using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
	public class ProductDto
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public int InventoryCount { get; set; }
        public long Price { get; set; }
        public double Discount { get; set; }
    }
}

