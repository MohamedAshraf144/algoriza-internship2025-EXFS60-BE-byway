using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Common
{
	public class PlatformStatsDto
	{
		public int TotalCourses { get; set; }
		public int TotalInstructors { get; set; }
		public int TotalCategories { get; set; }
		public int TotalUsers { get; set; }
		public int TotalOrders { get; set; }
		public decimal MonthlyRevenue { get; set; }
	}
}
