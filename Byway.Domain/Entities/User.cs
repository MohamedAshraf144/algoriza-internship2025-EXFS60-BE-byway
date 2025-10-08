using Byway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Domain.Entities
{
	public class User : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public UserRole Role { get; set; } = UserRole.Student;
		public bool IsEmailConfirmed { get; set; } = false;

		// Navigation Properties
		public virtual Cart Cart { get; set; }
		public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

	}
}
