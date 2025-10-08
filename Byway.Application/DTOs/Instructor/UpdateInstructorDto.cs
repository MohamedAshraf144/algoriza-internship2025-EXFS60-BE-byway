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
	public class UpdateInstructorDto
	{
		[StringLength(100)]
		public string? Name { get; set; }

		[StringLength(1000)]
		public string? Bio { get; set; }

		public IFormFile? Image { get; set; }

		public JobTitle? JobTitle { get; set; }

		[Range(0, 5)]
		public decimal? Rating { get; set; }
	}
}
