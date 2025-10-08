using Byway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class Order : BaseEntity
	{
		public int UserId { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal FinalAmount { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public string PaymentMethod { get; set; } = "";
		public string Notes { get; set; } = "";

		// Navigation Properties
		public virtual User User { get; set; }
		public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}
}
