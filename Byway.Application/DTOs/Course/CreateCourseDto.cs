using Byway.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Course
{
	public class CreateCourseDto
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		[Required]
		[StringLength(2000)]
		public string Description { get; set; }

		public IFormFile Image { get; set; }

		[Required]
		[Range(0, double.MaxValue)]
		public decimal Price { get; set; }

		[Required]
		public Level Level { get; set; }

		[Required]
		[Range(1, 1000)]
		public int Duration { get; set; }

		[Required]
		[StringLength(1000)]
		public string Requirements { get; set; } = "";

		[Required]
		[StringLength(1000)]
		public string LearningOutcomes { get; set; } = "";

		[Required]
		public int CategoryId { get; set; }

		[Required]
		public int InstructorId { get; set; }

		[Range(0, 5)]
		public decimal Rating { get; set; } = 0;
	}
}
