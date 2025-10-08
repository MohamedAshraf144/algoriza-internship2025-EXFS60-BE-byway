using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Course
{
	public class CourseDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public decimal Price { get; set; }
		public decimal Rating { get; set; }
		public string Level { get; set; }
		public int Duration { get; set; }
		public string Requirements { get; set; }
		public string LearningOutcomes { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int InstructorId { get; set; }
		public string InstructorName { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
