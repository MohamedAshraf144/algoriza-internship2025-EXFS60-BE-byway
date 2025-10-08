using Byway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Interfaces.IRepositories
{
	public interface IBaseRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task DeleteAsync(int id);
		Task<bool> ExistsAsync(int id);
		Task<int> CountAsync();
	}
}
