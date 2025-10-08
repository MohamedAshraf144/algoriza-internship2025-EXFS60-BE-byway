using AutoMapper;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using Byway.Application.DTOs.Auth;
using Byway.Application.DTOs.Course;
using Byway.Application.DTOs.Category;
using Byway.Application.DTOs.Instructor;
using Byway.Application.DTOs.Cart;
using Byway.Application.DTOs.Order;

namespace Byway.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<User, AuthResponseDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => UserRole.Student))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => false));

            // Course Mappings
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CourseDetailsDto>();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();

            // Category Mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Instructor Mappings
            CreateMap<Instructor, InstructorDto>();
            CreateMap<CreateInstructorDto, Instructor>();
            CreateMap<UpdateInstructorDto, Instructor>();

            // Cart Mappings
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.TaxAmount, opt => opt.Ignore())
                .ForMember(dest => dest.FinalTotal, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            CreateMap<CartItem, CartItemDto>();

            // Order Mappings
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}