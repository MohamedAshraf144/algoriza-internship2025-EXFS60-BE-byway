using Byway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class Course : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public decimal Price { get; set; }
		public decimal Rating { get; set; } = 0;
		public Level Level { get; set; }
		public int Duration { get; set; } // in hours
		public string Requirements { get; set; } = "";
		public string LearningOutcomes { get; set; } = "";

		// Foreign Keys
		public int CategoryId { get; set; }
		public int InstructorId { get; set; }

		// Navigation Properties
		public virtual Category Category { get; set; }
		public virtual Instructor Instructor { get; set; }
		public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
		public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}
}
