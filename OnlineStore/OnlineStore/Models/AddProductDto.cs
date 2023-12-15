using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
	public class AddProductDto
	{
		[Required]
		[MaxLength(40)]
		public string Title { get; set; }

		[Required]
		public long Price { get; set; }

		public double Discount { get; set; }
	}
}

