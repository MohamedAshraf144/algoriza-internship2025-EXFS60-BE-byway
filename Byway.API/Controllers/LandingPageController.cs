using Microsoft.AspNetCore.Mvc;
using Byway.Application.DTOs.Common;
using Byway.Application.Services;
using Byway.Domain.Interfaces.IRepositories;
using Byway.Domain.Entities;
using AutoMapper;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LandingPageController : ControllerBase
	{
		private readonly CourseService _courseService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public LandingPageController(CourseService courseService, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_courseService = courseService;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<LandingPageDto>> GetLandingPageData()
		{
			try
			{
				var topCourses = await _courseService.GetTopCoursesAsync(6);
				var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
				var instructors = await _unitOfWork.Repository<Instructor>().GetAllAsync();

				var landingPageData = new LandingPageDto
				{
					TopCourses = topCourses,
					Categories = _mapper.Map<IEnumerable<Byway.Application.DTOs.Category.CategoryDto>>(categories.Take(8)),
					TopInstructors = _mapper.Map<IEnumerable<Byway.Application.DTOs.Instructor.InstructorDto>>(instructors.Take(4))
				};

				return Ok(landingPageData);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("stats")]
		public async Task<ActionResult<PlatformStatsDto>> GetPlatformStats()
		{
			try
			{
				var userCount = await _unitOfWork.Repository<User>().CountAsync();
				var courseCount = await _unitOfWork.Repository<Course>().CountAsync();
				var instructorCount = await _unitOfWork.Repository<Instructor>().CountAsync();
				var categoryCount = await _unitOfWork.Repository<Category>().CountAsync();

				// Get this month's orders count
				var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				var allOrders = await _unitOfWork.Repository<Order>().GetAllAsync();
				var thisMonthOrders = allOrders.Where(o => o.OrderDate >= startOfMonth).Count();

				var stats = new PlatformStatsDto
				{
					TotalUsers = userCount,
					TotalCourses = courseCount,
					TotalInstructors = instructorCount,
					TotalCategories = categoryCount,
					TotalOrders = thisMonthOrders
				};

				return Ok(stats);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
