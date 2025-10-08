using AutoMapper;
using Byway.Application.DTOs.Cart;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
	public class CartService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CartService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CartDto> GetUserCartAsync(int userId)
		{
			var userCarts = await _unitOfWork.Repository<Cart>()
				.FindAsync(c => c.UserId == userId);
			var cart = userCarts.FirstOrDefault();

			if (cart == null)
			{
				cart = new Cart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow
				};
				await _unitOfWork.Repository<Cart>().AddAsync(cart);
				await _unitOfWork.SaveChangesAsync();
			}

			// Get cart items with course details
			var cartItems = await _unitOfWork.Repository<CartItem>().FindAsync(ci => ci.CartId == cart.Id);
			var cartDto = _mapper.Map<CartDto>(cart);

			// Build cart items with course details
			var cartItemDtos = new List<CartItemDto>();
			decimal subtotal = 0;

			foreach (var cartItem in cartItems)
			{
				var course = await _unitOfWork.Repository<Course>().GetByIdAsync(cartItem.CourseId);
				if (course != null)
				{
					var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(course.InstructorId);

					var cartItemDto = new CartItemDto
					{
						Id = cartItem.Id,
						CourseId = course.Id,
						CourseTitle = course.Title,
						CourseImage = course.ImagePath,
						CoursePrice = course.Price,
						InstructorName = instructor?.Name ?? "Unknown",
						Duration = course.Duration,
						AddedAt = cartItem.CreatedAt
					};

					cartItemDtos.Add(cartItemDto);
					subtotal += course.Price;
				}
			}

			cartDto.Items = cartItemDtos;

			// Add tax calculation (15%)
			cartDto.TotalPrice = subtotal;
			cartDto.TaxAmount = subtotal * 0.15m;
			cartDto.FinalTotal = cartDto.TotalPrice + cartDto.TaxAmount;

			return cartDto;
		}

		public async Task<bool> AddToCartAsync(int userId, int courseId)
		{
			var userCarts = await _unitOfWork.Repository<Cart>()
				.FindAsync(c => c.UserId == userId);
			var cart = userCarts.FirstOrDefault();

			if (cart == null)
			{
				cart = new Cart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow
				};
				await _unitOfWork.Repository<Cart>().AddAsync(cart);
				await _unitOfWork.SaveChangesAsync();
			}

			var existingItem = await _unitOfWork.Repository<CartItem>()
				.FindAsync(ci => ci.CartId == cart.Id && ci.CourseId == courseId);

			if (existingItem.Any())
				return false; // Item already in cart

			var cartItem = new CartItem
			{
				CartId = cart.Id,
				CourseId = courseId,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Repository<CartItem>().AddAsync(cartItem);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> RemoveFromCartAsync(int userId, int courseId)
		{
			var userCarts = await _unitOfWork.Repository<Cart>()
				.FindAsync(c => c.UserId == userId);
			var cart = userCarts.FirstOrDefault();

			if (cart == null)
				return false;

			var cartItems = await _unitOfWork.Repository<CartItem>()
				.FindAsync(ci => ci.CartId == cart.Id && ci.CourseId == courseId);
			var cartItem = cartItems.FirstOrDefault();

			if (cartItem == null)
				return false;

			await _unitOfWork.Repository<CartItem>().DeleteAsync(cartItem.Id);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> ClearCartAsync(int userId)
		{
			var userCarts = await _unitOfWork.Repository<Cart>()
				.FindAsync(c => c.UserId == userId);
			var cart = userCarts.FirstOrDefault();

			if (cart == null)
				return false;

			var cartItems = await _unitOfWork.Repository<CartItem>()
				.FindAsync(ci => ci.CartId == cart.Id);

			foreach (var item in cartItems)
			{
				await _unitOfWork.Repository<CartItem>().DeleteAsync(item.Id);
			}

			await _unitOfWork.SaveChangesAsync();
			return true;
		}
	}
}
