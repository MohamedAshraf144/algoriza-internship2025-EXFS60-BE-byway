using Byway.Application.DTOs.Category;
using Byway.Application.DTOs.Course;
using Byway.Application.DTOs.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Common
{
	public class LandingPageDto
	{
		public PlatformStatsDto Stats { get; set; } = new PlatformStatsDto();
		public IEnumerable<CourseDto> TopCourses { get; set; } = new List<CourseDto>();
		public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
		public IEnumerable<InstructorDto> TopInstructors { get; set; } = new List<InstructorDto>();
	}
}
