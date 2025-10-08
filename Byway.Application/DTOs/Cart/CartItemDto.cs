using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Cart
{
	public class CartItemDto
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public string CourseTitle { get; set; }
		public string CourseImage { get; set; }
		public decimal CoursePrice { get; set; }
		public string InstructorName { get; set; }
		public int Duration { get; set; }
		public DateTime AddedAt { get; set; }
	}
}
