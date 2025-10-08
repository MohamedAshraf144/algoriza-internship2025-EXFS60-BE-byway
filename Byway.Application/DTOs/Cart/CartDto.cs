using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Cart
{

	public class CartDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public IEnumerable<CartItemDto> Items { get; set; } = new List<CartItemDto>();
		public decimal TotalPrice { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal FinalTotal { get; set; }
		public int ItemsCount => Items?.Count() ?? 0;
	}
}
