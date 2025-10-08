using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Course
{
	public class CourseDetailsDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string Description { get; set; } = "";
		public string ImagePath { get; set; } = "";
		public decimal Price { get; set; }
		public decimal Rating { get; set; }
		public string Level { get; set; } = "";
		public int Duration { get; set; }
		public string Requirements { get; set; } = "";
		public string LearningOutcomes { get; set; } = "";
		public DateTime CreatedAt { get; set; }

		// Category Info
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = "";
		public string CategoryImage { get; set; } = "";

		// Instructor Info
		public int InstructorId { get; set; }
		public string InstructorName { get; set; } = "";
		public string InstructorBio { get; set; } = "";
		public string InstructorImage { get; set; } = "";
		public string InstructorJobTitle { get; set; } = "";
		public decimal InstructorRating { get; set; }

		// Similar Courses
		public IEnumerable<CourseDto> SimilarCourses { get; set; } = new List<CourseDto>();
	}
}
