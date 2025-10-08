using Byway.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Instructor
{
	public class CreateInstructorDto
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[StringLength(1000)]
		public string Bio { get; set; }

		public IFormFile Image { get; set; }

		[Required]
		public JobTitle JobTitle { get; set; }

		[Range(0, 5)]
		public decimal Rating { get; set; } = 0;
	}
}
