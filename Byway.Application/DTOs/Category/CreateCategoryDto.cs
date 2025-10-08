using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Category
{
	public class CreateCategoryDto
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; } = "";

		[Required]
		[StringLength(500)]
		public string ImagePath { get; set; } = "";
	}
}
