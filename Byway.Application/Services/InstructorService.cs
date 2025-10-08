using AutoMapper;
using Byway.Application.DTOs.Common;
using Byway.Application.DTOs.Instructor;
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
	public class InstructorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;

		public InstructorService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_fileService = fileService;
		}

		public async Task<IEnumerable<InstructorDto>> GetAllInstructorsAsync()
		{
			var instructors = await _unitOfWork.Repository<Instructor>().GetAllAsync();
			return _mapper.Map<IEnumerable<InstructorDto>>(instructors);
		}

		public async Task<PaginatedResultDto<InstructorDto>> GetAllInstructorsAsync(int page, int pageSize, string search = "", string sortBy = "name")
		{
			var allInstructors = await _unitOfWork.Repository<Instructor>().GetAllAsync();
			var filteredInstructors = allInstructors.AsQueryable();

			// Apply search filter
			if (!string.IsNullOrEmpty(search))
			{
				filteredInstructors = filteredInstructors.Where(i =>
					i.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
					i.JobTitle.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
			}

			// Apply sorting
			filteredInstructors = sortBy.ToLower() switch
			{
				"jobtitle" => filteredInstructors.OrderBy(i => i.JobTitle),
				"rating" => filteredInstructors.OrderByDescending(i => i.Rating),
				"date" => filteredInstructors.OrderByDescending(i => i.CreatedAt),
				_ => filteredInstructors.OrderBy(i => i.Name)
			};

			var totalCount = filteredInstructors.Count();
			var pagedInstructors = filteredInstructors.Skip((page - 1) * pageSize).Take(pageSize);

			var instructorDtos = _mapper.Map<IEnumerable<InstructorDto>>(pagedInstructors);

			return new PaginatedResultDto<InstructorDto>
			{
				Items = instructorDtos,
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize
			};
		}

		public async Task<InstructorDto> GetInstructorByIdAsync(int id)
		{
			var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(id);
			if (instructor == null)
				throw new Exception("Instructor not found");

			return _mapper.Map<InstructorDto>(instructor);
		}

		public async Task<InstructorDto> CreateInstructorAsync(CreateInstructorDto dto)
		{
			// Save image first
			var imagePath = await _fileService.SaveImageAsync(dto.Image, "instructors");
			
			// Create instructor entity manually to avoid AutoMapper issues with IFormFile
			var instructor = new Instructor
			{
				Name = dto.Name,
				Bio = dto.Bio,
				ImagePath = imagePath,
				JobTitle = dto.JobTitle,
				Rating = dto.Rating,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.Repository<Instructor>().AddAsync(instructor);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<InstructorDto>(instructor);
		}

		public async Task<InstructorDto> UpdateInstructorAsync(int id, UpdateInstructorDto dto)
		{
			var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(id);
			if (instructor == null)
				throw new Exception("Instructor not found");

			// Update only provided fields
			if (!string.IsNullOrEmpty(dto.Name))
				instructor.Name = dto.Name;
			
			if (!string.IsNullOrEmpty(dto.Bio))
				instructor.Bio = dto.Bio;
			
			if (dto.JobTitle.HasValue)
				instructor.JobTitle = dto.JobTitle.Value;

			if (dto.Rating.HasValue)
				instructor.Rating = dto.Rating.Value;

			// Handle image upload if provided
			if (dto.Image != null && dto.Image.Length > 0)
			{
				var imagePath = await _fileService.SaveImageAsync(dto.Image, "instructors");
				instructor.ImagePath = imagePath;
			}

			instructor.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.Repository<Instructor>().UpdateAsync(instructor);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<InstructorDto>(instructor);
		}

		public async Task<bool> DeleteInstructorAsync(int id)
		{
			var instructor = await _unitOfWork.Repository<Instructor>().GetByIdAsync(id);
			if (instructor == null)
				return false;

			// Check if instructor has courses
			var courses = await _unitOfWork.Repository<Course>().FindAsync(c => c.InstructorId == id);
			if (courses.Any())
				throw new Exception("Cannot delete instructor who has courses assigned");

			await _unitOfWork.Repository<Instructor>().DeleteAsync(id);
			await _unitOfWork.SaveChangesAsync();

			return true;
		}
	}
}
