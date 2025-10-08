using Microsoft.AspNetCore.Mvc;
using Byway.Application.DTOs.Category;
using Byway.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly CategoryService _categoryService;

		public CategoriesController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
		{
			try
			{
				var categories = await _categoryService.GetAllCategoriesAsync();
				return Ok(categories);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDto>> GetCategory(int id)
		{
			try
			{
				var category = await _categoryService.GetCategoryByIdAsync(id);
				return Ok(category);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("seed")]
		public async Task<ActionResult> SeedCategories()
		{
			try
			{
				await _categoryService.SeedCategoriesAsync();
				return Ok(new { message = "Categories seeded successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto dto)
		{
			try
			{
				var category = await _categoryService.CreateCategoryAsync(dto);
				return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
		{
			try
			{
				var category = await _categoryService.UpdateCategoryAsync(id, dto);
				return Ok(category);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> DeleteCategory(int id)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
