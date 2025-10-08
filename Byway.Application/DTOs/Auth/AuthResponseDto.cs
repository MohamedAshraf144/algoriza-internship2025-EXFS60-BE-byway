using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Auth
{
	public class AuthResponseDto
	{
		public bool Success { get; set; } = true;
		public string Message { get; set; } = "";
		public string Token { get; set; } = "";
		public int UserId { get; set; }
		public string Email { get; set; } = "";
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string Role { get; set; } = "";
	}
}
