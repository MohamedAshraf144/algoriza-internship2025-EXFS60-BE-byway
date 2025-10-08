using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Order
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal FinalAmount { get; set; }
		public string Status { get; set; }
		public DateTime OrderDate { get; set; }
		public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
	}
}
