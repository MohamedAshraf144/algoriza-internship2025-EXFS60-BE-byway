using Microsoft.AspNetCore.Mvc;
using Byway.Domain.Interfaces.IRepositories;
using Byway.Domain.Entities;
using Byway.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public AdminController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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

				// Get this month's orders count and revenue
				var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				var allOrders = await _unitOfWork.Repository<Order>().GetAllAsync();
				var thisMonthOrders = allOrders.Where(o => o.OrderDate >= startOfMonth).Count();
				var thisMonthRevenue = allOrders.Where(o => o.OrderDate >= startOfMonth).Sum(o => o.TotalAmount);

				var stats = new PlatformStatsDto
				{
					TotalUsers = userCount,
					TotalCourses = courseCount,
					TotalInstructors = instructorCount,
					TotalCategories = categoryCount,
					TotalOrders = thisMonthOrders,
					MonthlyRevenue = thisMonthRevenue
				};

				return Ok(stats);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("users")]
		public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
		{
			try
			{
				var users = await _unitOfWork.Repository<User>().GetAllAsync();
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("orders")]
		public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
		{
			try
			{
				var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
