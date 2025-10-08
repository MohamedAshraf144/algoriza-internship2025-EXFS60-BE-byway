using Byway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Interfaces.IRepositories
{
	public interface IUnitOfWork : IDisposable
	{
		IBaseRepository<T> Repository<T>() where T : BaseEntity;
		Task<int> SaveAsync();
		Task<int> SaveChangesAsync();
	}
}
