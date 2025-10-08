using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Category
{
	public class CategoryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImagePath { get; set; }
		public int CoursesCount { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
