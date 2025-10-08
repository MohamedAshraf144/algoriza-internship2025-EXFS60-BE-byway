using Microsoft.AspNetCore.Mvc;
using Byway.Application.Services;
using Byway.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrdersController : ControllerBase
	{
		private readonly OrderService _orderService;

		public OrdersController(OrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(int userId)
		{
			try
			{
				var orders = await _orderService.GetUserOrdersAsync(userId);
				return Ok(orders);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderDto>> GetOrder(int id)
		{
			try
			{
				var order = await _orderService.GetOrderByIdAsync(id);
				return Ok(order);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto dto)
		{
			try
			{
				var order = await _orderService.CreateOrderAsync(dto);
				return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}/status")]
		public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] string status)
		{
			try
			{
				await _orderService.UpdateOrderStatusAsync(id, status);
				return Ok(new { message = "Order status updated successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
