using Microsoft.EntityFrameworkCore;
using Byway.Domain.Entities;
using Byway.Domain.Interfaces.IRepositories;
using Byway.Infrastructure.Data;
using System.Linq.Expressions;

namespace Byway.Infrastructure.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
	{
		protected readonly ApplicationDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public virtual async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
		{
			return await _dbSet
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync();
		}

		public virtual async Task<T> AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			return entity;
		}

		public virtual async Task<T> UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			return entity;
		}

		public virtual async Task DeleteAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
			}
		}

		public virtual async Task<bool> ExistsAsync(int id)
		{
			return await _dbSet.AnyAsync(e => e.Id == id);
		}

		public virtual async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}
	}
}