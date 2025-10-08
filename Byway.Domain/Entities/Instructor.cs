using Byway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class Instructor : BaseEntity
	{
		public string Name { get; set; }
		public string Bio { get; set; }
		public string ImagePath { get; set; }
		public JobTitle JobTitle { get; set; }
		public decimal Rating { get; set; } = 0;

		// Navigation Properties
		public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
	}
}
