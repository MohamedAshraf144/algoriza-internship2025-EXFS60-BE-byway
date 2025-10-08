using Byway.Domain.Entities;
using Byway.Domain.Interfaces.IRepositories;
using Byway.Infrastructure.Data;

namespace Byway.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly Dictionary<Type, object> _repositories = new();

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public IBaseRepository<T> Repository<T>() where T : BaseEntity
		{
			if (_repositories.ContainsKey(typeof(T)))
			{
				return (IBaseRepository<T>)_repositories[typeof(T)];
			}

			var repository = new BaseRepository<T>(_context);
			_repositories[typeof(T)] = repository;
			return repository;
		}

		public async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}