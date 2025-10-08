using Microsoft.AspNetCore.Mvc;
using Byway.Application.Services;
using Byway.Application.DTOs.Course;
using Byway.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly CourseService _courseService;

		public CoursesController(CourseService courseService)
		{
			_courseService = courseService;
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedResultDto<CourseDto>>> GetCourses(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string search = "",
			[FromQuery] int? categoryId = null,
			[FromQuery] decimal? minPrice = null,
			[FromQuery] decimal? maxPrice = null,
			[FromQuery] double? minRating = null,
			[FromQuery] string sortBy = "title")
		{
			try
			{
				var result = await _courseService.GetAllCoursesAsync(page, pageSize, search, categoryId, minPrice, maxPrice, minRating, sortBy);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CourseDetailsDto>> GetCourse(int id)
		{
			try
			{
				var course = await _courseService.GetCourseByIdAsync(id);
				return Ok(course);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CourseDto>> CreateCourse([FromForm] CreateCourseDto dto)
		{
			try
			{
				// Try to manually parse rating from form data if it's 0
				if (dto.Rating == 0 && Request.Form.ContainsKey("Rating"))
				{
					var ratingString = Request.Form["Rating"].ToString();
					if (decimal.TryParse(ratingString, out decimal parsedRating))
					{
						dto.Rating = parsedRating;
					}
				}
				
				var course = await _courseService.CreateCourseAsync(dto);
				return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromForm] UpdateCourseDto dto)
		{
			try
			{
				var course = await _courseService.UpdateCourseAsync(id, dto);
				return Ok(course);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> DeleteCourse(int id)
		{
			try
			{
				await _courseService.DeleteCourseAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("top")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetTopCourses([FromQuery] int count = 6)
		{
			try
			{
				var courses = await _courseService.GetTopCoursesAsync(count);
				return Ok(courses);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}/similar")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetSimilarCourses(int id, [FromQuery] int count = 4)
		{
			try
			{
				var courses = await _courseService.GetSimilarCoursesAsync(id, count);
				return Ok(courses);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
