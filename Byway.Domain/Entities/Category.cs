using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public string ImagePath { get; set; }

		// Navigation Properties
		public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
	}
}
