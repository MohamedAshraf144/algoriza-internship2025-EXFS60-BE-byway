using AutoMapper;
using Byway.Application.DTOs.Category;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Services
{
	public class CategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
		{
			var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
			var categoryDtos = new List<CategoryDto>();

			foreach (var category in categories)
			{
				var categoryDto = _mapper.Map<CategoryDto>(category);
				var courses = await _unitOfWork.Repository<Course>().FindAsync(c => c.CategoryId == category.Id);
				categoryDto.CoursesCount = courses.Count();
				categoryDtos.Add(categoryDto);
			}

			return categoryDtos;
		}

		public async Task<CategoryDto> GetCategoryByIdAsync(int id)
		{
			var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
			if (category == null)
				throw new Exception("Category not found");

			var categoryDto = _mapper.Map<CategoryDto>(category);
			var courses = await _unitOfWork.Repository<Course>().FindAsync(c => c.CategoryId == category.Id);
			categoryDto.CoursesCount = courses.Count();

			return categoryDto;
		}

		public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
		{
			var category = new Category
			{
				Name = dto.Name,
				ImagePath = dto.ImagePath,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Repository<Category>().AddAsync(category);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CategoryDto>(category);
		}

		public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
		{
			var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
			if (category == null)
				throw new Exception("Category not found");

			category.Name = dto.Name;
			category.ImagePath = dto.ImagePath;
			category.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.Repository<Category>().UpdateAsync(category);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CategoryDto>(category);
		}

		public async Task DeleteCategoryAsync(int id)
		{
			var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
			if (category == null)
				throw new Exception("Category not found");

			// Check if category has courses
			var courses = await _unitOfWork.Repository<Course>().FindAsync(c => c.CategoryId == id);
			if (courses.Any())
				throw new Exception("Cannot delete category that has courses");

			await _unitOfWork.Repository<Category>().DeleteAsync(category.Id);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task SeedCategoriesAsync()
		{
			var existingCategories = await _unitOfWork.Repository<Category>().GetAllAsync();
			if (existingCategories.Any())
				return; // Categories already seeded

			var categories = new List<Category>
			{
				new Category { Name = "Web Development", ImagePath = "/images/categories/web-development.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Mobile Development", ImagePath = "/images/categories/mobile-development.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Data Science", ImagePath = "/images/categories/data-science.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Programming Languages", ImagePath = "/images/categories/programming.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Database", ImagePath = "/images/categories/database.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "DevOps", ImagePath = "/images/categories/devops.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "UI/UX Design", ImagePath = "/images/categories/ui-ux.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Machine Learning", ImagePath = "/images/categories/machine-learning.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Cybersecurity", ImagePath = "/images/categories/cybersecurity.png", CreatedAt = DateTime.UtcNow },
				new Category { Name = "Cloud Computing", ImagePath = "/images/categories/cloud.png", CreatedAt = DateTime.UtcNow }
			};

			foreach (var category in categories)
			{
				await _unitOfWork.Repository<Category>().AddAsync(category);
			}

			await _unitOfWork.SaveChangesAsync();
		}
	}
}