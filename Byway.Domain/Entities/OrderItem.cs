using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class OrderItem : BaseEntity
	{
		public int OrderId { get; set; }
		public int CourseId { get; set; }
		public decimal Price { get; set; } // Price at time of purchase

		// Navigation Properties
		public virtual Order Order { get; set; }
		public virtual Course Course { get; set; }
	}
}
