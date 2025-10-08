using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Order
{
	public class CreateOrderDto
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		public string PaymentMethod { get; set; }

		public string Notes { get; set; }
	}
}
