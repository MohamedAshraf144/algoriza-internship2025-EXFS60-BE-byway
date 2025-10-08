using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Order
{
	public class OrderItemDto
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public string CourseTitle { get; set; }
		public string CourseImage { get; set; }
		public decimal Price { get; set; }
		public string InstructorName { get; set; }
		public int Duration { get; set; }
		public string Level { get; set; }
		public decimal Rating { get; set; }
		public string CategoryName { get; set; }
	}
}
