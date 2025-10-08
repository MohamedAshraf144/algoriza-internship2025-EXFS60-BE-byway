using AutoMapper;
using Byway.Application.DTOs.Order;
using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using Byway.Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
	public class OrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;

		public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_emailService = emailService;
		}

		public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
		{
			var orders = await _unitOfWork.Repository<Order>()
				.FindAsync(o => o.UserId == userId);
			
			var orderDtos = new List<OrderDto>();
			
			foreach (var order in orders)
			{
				var orderDto = _mapper.Map<OrderDto>(order);
				
				// Get order items with course details
				var orderItems = await _unitOfWork.Repository<OrderItem>()
					.FindAsync(oi => oi.OrderId == order.Id);
				
				var orderItemDtos = new List<OrderItemDto>();
				
				foreach (var orderItem in orderItems)
				{
					var course = await _unitOfWork.Repository<Course>().GetByIdAsync(orderItem.CourseId);
					var instructor = course != null ? await _unitOfWork.Repository<Instructor>().GetByIdAsync(course.InstructorId) : null;
					var category = course != null ? await _unitOfWork.Repository<Category>().GetByIdAsync(course.CategoryId) : null;
					
					var orderItemDto = new OrderItemDto
					{
						Id = orderItem.Id,
						CourseId = orderItem.CourseId,
						CourseTitle = course?.Title ?? "Unknown Course",
						CourseImage = course?.ImagePath ?? "/placeholder-course.jpg",
						Price = orderItem.Price,
						InstructorName = instructor?.Name ?? "Unknown Instructor",
						Duration = course?.Duration ?? 0,
						Level = course != null ? course.Level.ToString() : "Beginner",
						Rating = course?.Rating ?? 0,
						CategoryName = category?.Name ?? "General"
					};
					
					orderItemDtos.Add(orderItemDto);
				}
				
				orderDto.Items = orderItemDtos;
				orderDtos.Add(orderDto);
			}
			
			return orderDtos;
		}

		public async Task<OrderDto> GetOrderByIdAsync(int id)
		{
			var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
			if (order == null)
				throw new Exception("Order not found");

			return _mapper.Map<OrderDto>(order);
		}

		public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
		{
			// Get user's cart
			var userCarts = await _unitOfWork.Repository<Cart>().FindAsync(c => c.UserId == dto.UserId);
			var cart = userCarts.FirstOrDefault();

			if (cart == null)
				throw new Exception("Cart not found");

			var cartItems = await _unitOfWork.Repository<CartItem>().FindAsync(ci => ci.CartId == cart.Id);
			if (!cartItems.Any())
				throw new Exception("Cart is empty");

			// Calculate totals
			var courses = new List<Course>();
			decimal subtotal = 0;

			foreach (var cartItem in cartItems)
			{
				var course = await _unitOfWork.Repository<Course>().GetByIdAsync(cartItem.CourseId);
				if (course != null)
				{
					courses.Add(course);
					subtotal += course.Price;
				}
			}

			var taxAmount = subtotal * 0.15m; // 15% tax
			var totalAmount = subtotal + taxAmount;

			// Create order
			var order = new Order
			{
				UserId = dto.UserId,
				OrderDate = DateTime.UtcNow,
				TotalAmount = subtotal,
				TaxAmount = taxAmount,
				FinalAmount = totalAmount,
				Status = OrderStatus.Completed,
				PaymentMethod = dto.PaymentMethod,
				Notes = dto.Notes ?? "",
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Repository<Order>().AddAsync(order);
			await _unitOfWork.SaveChangesAsync();

			// Create order items
			foreach (var course in courses)
			{
				var orderItem = new OrderItem
				{
					OrderId = order.Id,
					CourseId = course.Id,
					Price = course.Price,
					CreatedAt = DateTime.UtcNow
				};
				await _unitOfWork.Repository<OrderItem>().AddAsync(orderItem);
			}

			// Clear cart after successful order
			foreach (var cartItem in cartItems)
			{
				await _unitOfWork.Repository<CartItem>().DeleteAsync(cartItem.Id);
			}

			await _unitOfWork.SaveChangesAsync();

			// Send payment confirmation email
			var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);
			if (user != null)
			{
				await _emailService.SendPaymentConfirmationEmailAsync(user.Email, user.FirstName, totalAmount);
			}

			return _mapper.Map<OrderDto>(order);
		}

		public async Task<bool> UpdateOrderStatusAsync(int id, string status)
		{
			var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
			if (order == null)
				return false;

			if (Enum.TryParse<OrderStatus>(status, out var orderStatus))
			{
				order.Status = orderStatus;
				order.UpdatedAt = DateTime.UtcNow;

				await _unitOfWork.Repository<Order>().UpdateAsync(order);
				await _unitOfWork.SaveChangesAsync();
				return true;
			}

			return false;
		}
	}
}
