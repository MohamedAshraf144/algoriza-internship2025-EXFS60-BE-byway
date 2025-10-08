using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class Cart : BaseEntity
	{
		public int UserId { get; set; }

		// Navigation Properties
		public virtual User User { get; set; }
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

		// Calculated Properties
		public decimal TotalPrice => CartItems?.Sum(x => x.Course.Price) ?? 0;
		public decimal TaxAmount => TotalPrice * 0.15m; // 15% tax
		public decimal FinalTotal => TotalPrice + TaxAmount;
	}
}
