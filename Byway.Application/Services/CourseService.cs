using AutoMapper;
using Byway.Application.DTOs.Common;
using Byway.Application.DTOs.Course;
using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
	public class CourseService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;

		public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_fileService = fileService;
		}

		public async Task<PaginatedResultDto<CourseDto>> GetAllCoursesAsync(int page, int pageSize, string search = "", int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minRating = null, string sortBy = "title")
		{
			var allCourses = await _unitOfWork.Repository<Course>().GetAllAsync();
			var filteredCourses = allCourses.AsQueryable();

			// Apply search filter
			if (!string.IsNullOrEmpty(search))
			{
				filteredCourses = filteredCourses.Where(c =>
					c.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
					c.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
			}

			// Apply category filter
			if (categoryId.HasValue)
			{
				filteredCourses = filteredCourses.Where(c => c.CategoryId == categoryId.Value);
			}

			// Apply price range filter
			if (minPrice.HasValue)
			{
				filteredCourses = filteredCourses.Where(c => c.Price >= minPrice.Value);
			}
			if (maxPrice.HasValue)
			{
				filteredCourses = filteredCourses.Where(c => c.Price <= maxPrice.Value);
			}

			// Apply rating filter
			if (minRating.HasValue)
			{
				filteredCourses = filteredCourses.Where(c => c.Rating >= (decimal)minRating.Value);
			}

			// Apply sorting
			filteredCourses = sortBy.ToLower() switch
			{
				"price" => filteredCourses.OrderBy(c => c.Price),
				"priceDesc" => filteredCourses.OrderByDescending(c => c.Price),
				"rating" => filteredCourses.OrderByDescending(c => c.Rating),
				"date" => filteredCourses.OrderByDescending(c => c.CreatedAt),
				_ => filteredCourses.OrderBy(c => c.Title)
			};

			var totalCount = filteredCourses.Count();
			var pagedCourses = filteredCourses.Skip((page - 1) * pageSize).Take(pageSize);

			// Map courses and populate category/instructor names
			var courseDtos = new List<CourseDto>();
			foreach (var course in pagedCourses)
			{
				var courseDto = _mapper.Map<CourseDto>(course);
				
				// Get category and instructor names
				var category = await _unitOfWork.Repository<Category>().GetByIdAsync(course.CategoryId);
				var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(course.InstructorId);
				
				courseDto.CategoryName = category?.Name ?? "Unknown Category";
				courseDto.InstructorName = instructor?.Name ?? "Unknown Instructor";
				
				courseDtos.Add(courseDto);
			}

			return new PaginatedResultDto<CourseDto>
			{
				Items = courseDtos,
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize
			};
		}

		public async Task<CourseDetailsDto> GetCourseByIdAsync(int id)
		{
			var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
			if (course == null)
				throw new Exception("Course not found");

			return _mapper.Map<CourseDetailsDto>(course);
		}

		public async Task<IEnumerable<CourseDto>> GetSimilarCoursesAsync(int courseId, int count)
		{
			var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);
			if (course == null)
				throw new Exception("Course not found");

			var similarCourses = await _unitOfWork.Repository<Course>()
				.FindAsync(c => c.CategoryId == course.CategoryId && c.Id != courseId);

			return _mapper.Map<IEnumerable<CourseDto>>(similarCourses.Take(count));
		}

		public async Task<IEnumerable<CourseDto>> GetTopCoursesAsync(int count)
		{
			var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
			var topCourses = courses.OrderByDescending(c => c.Rating).Take(count);

			return _mapper.Map<IEnumerable<CourseDto>>(topCourses);
		}

		public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
		{
			
			// Save image first
			var imagePath = await _fileService.SaveImageAsync(dto.Image, "courses");
			
			// Create course entity manually to avoid AutoMapper issues with IFormFile
			var course = new Course
			{
				Title = dto.Title,
				Description = dto.Description,
				ImagePath = imagePath,
				Price = dto.Price,
				Level = dto.Level,
				Duration = dto.Duration,
				Requirements = dto.Requirements,
				LearningOutcomes = dto.LearningOutcomes,
				CategoryId = dto.CategoryId,
				InstructorId = dto.InstructorId,
				Rating = dto.Rating,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Repository<Course>().AddAsync(course);
			await _unitOfWork.SaveChangesAsync();

			// Get the created course with related data
			var createdCourse = await _unitOfWork.Repository<Course>().GetByIdAsync(course.Id);
			var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createdCourse.CategoryId);
			var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(createdCourse.InstructorId);

			var courseDto = _mapper.Map<CourseDto>(createdCourse);
			courseDto.CategoryName = category?.Name ?? "Unknown Category";
			courseDto.InstructorName = instructor?.Name ?? "Unknown Instructor";

			return courseDto;
		}

		public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto)
		{
			var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
			if (course == null)
				throw new Exception("Course not found");

			// Check if course has been purchased
			var orderItems = await _unitOfWork.Repository<OrderItem>().FindAsync(oi => oi.CourseId == id);
			if (orderItems.Any())
				throw new Exception("Cannot update course that has been purchased");

			// Update only provided fields
			if (!string.IsNullOrEmpty(dto.Title))
				course.Title = dto.Title;
			
			if (!string.IsNullOrEmpty(dto.Description))
				course.Description = dto.Description;
			
			if (dto.Price.HasValue)
				course.Price = dto.Price.Value;
			
			if (dto.Level.HasValue)
				course.Level = dto.Level.Value;
			
			if (dto.Duration.HasValue)
				course.Duration = dto.Duration.Value;
			
			if (!string.IsNullOrEmpty(dto.Requirements))
				course.Requirements = dto.Requirements;
			
			if (!string.IsNullOrEmpty(dto.LearningOutcomes))
				course.LearningOutcomes = dto.LearningOutcomes;
			
			if (dto.CategoryId.HasValue)
				course.CategoryId = dto.CategoryId.Value;
			
			if (dto.InstructorId.HasValue)
				course.InstructorId = dto.InstructorId.Value;
			
			if (dto.Rating.HasValue)
				course.Rating = dto.Rating.Value;

			// Handle image upload if provided
			if (dto.Image != null && dto.Image.Length > 0)
			{
				var imagePath = await _fileService.SaveImageAsync(dto.Image, "courses");
				course.ImagePath = imagePath;
			}

			course.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.Repository<Course>().UpdateAsync(course);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CourseDto>(course);
		}

		public async Task<bool> DeleteCourseAsync(int id)
		{
			var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
			if (course == null)
				return false;

			// Check if course has been purchased
			var orderItems = await _unitOfWork.Repository<OrderItem>().FindAsync(oi => oi.CourseId == id);
			if (orderItems.Any())
				throw new Exception("Cannot delete course that has been purchased");

			await _unitOfWork.Repository<Course>().DeleteAsync(id);
			await _unitOfWork.SaveChangesAsync();

			return true;
		}
	}
}
