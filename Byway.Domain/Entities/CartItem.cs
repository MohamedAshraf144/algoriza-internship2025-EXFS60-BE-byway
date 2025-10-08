using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class CartItem : BaseEntity
	{
		public int CartId { get; set; }
		public int CourseId { get; set; }

		// Navigation Properties
		public virtual Cart Cart { get; set; }
		public virtual Course Course { get; set; }
	}
}
