using Microsoft.AspNetCore.Mvc;
using Byway.Application.Services;
using Byway.Application.DTOs.Common;
using Byway.Application.DTOs.Instructor;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstructorsController : ControllerBase
	{
		private readonly InstructorService _instructorService;

		public InstructorsController(InstructorService instructorService)
		{
			_instructorService = instructorService;
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedResultDto<InstructorDto>>> GetInstructors(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string search = "",
			[FromQuery] string sortBy = "name")
		{
			try
			{
				var result = await _instructorService.GetAllInstructorsAsync(page, pageSize, search, sortBy);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<InstructorDto>> GetInstructor(int id)
		{
			try
			{
				var instructor = await _instructorService.GetInstructorByIdAsync(id);
				return Ok(instructor);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<InstructorDto>> CreateInstructor([FromForm] CreateInstructorDto dto)
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
				
				var instructor = await _instructorService.CreateInstructorAsync(dto);
				return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, instructor);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<InstructorDto>> UpdateInstructor(int id, [FromForm] UpdateInstructorDto dto)
		{
			try
			{
				var instructor = await _instructorService.UpdateInstructorAsync(id, dto);
				return Ok(instructor);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> DeleteInstructor(int id)
		{
			try
			{
				await _instructorService.DeleteInstructorAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
