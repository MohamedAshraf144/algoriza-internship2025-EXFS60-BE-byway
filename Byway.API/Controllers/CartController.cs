using Microsoft.AspNetCore.Mvc;
using Byway.Application.Services;
using Byway.Application.DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Byway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CartController : ControllerBase
	{
		private readonly CartService _cartService;

		public CartController(CartService cartService)
		{
			_cartService = cartService;
		}

	[HttpGet("{userId}")]
	public async Task<ActionResult<CartDto>> GetCart(int userId)
	{
		try
		{
			Console.WriteLine($"🛒 [CartController] GetCart called for userId: {userId}");
			var cart = await _cartService.GetUserCartAsync(userId);
			Console.WriteLine($"✅ [CartController] Cart retrieved successfully with {cart.ItemsCount} items");
			return Ok(cart);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ [CartController] Error getting cart: {ex.Message}");
			Console.WriteLine($"❌ [CartController] Stack trace: {ex.StackTrace}");
			return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
		}
	}

		[HttpPost("{userId}/items")]
		public async Task<ActionResult> AddToCart(int userId, [FromBody] AddToCartRequest request)
		{
			try
			{
				var success = await _cartService.AddToCartAsync(userId, request.CourseId);
				return Ok(new { success = success, message = "Item added to cart successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		public class AddToCartRequest
		{
			public int CourseId { get; set; }
		}

		[HttpDelete("{userId}/items/{courseId}")]
		public async Task<ActionResult> RemoveFromCart(int userId, int courseId)
		{
			try
			{
				var success = await _cartService.RemoveFromCartAsync(userId, courseId);
				return Ok(new { success = success, message = "Item removed from cart successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{userId}")]
		public async Task<ActionResult> ClearCart(int userId)
		{
			try
			{
				var success = await _cartService.ClearCartAsync(userId);
				return Ok(new { success = success, message = "Cart cleared successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
