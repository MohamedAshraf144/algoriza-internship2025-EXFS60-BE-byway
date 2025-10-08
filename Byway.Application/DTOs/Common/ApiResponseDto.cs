using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs.Common
{
	public class ApiResponseDto<T>
	{
		public bool Success { get; set; }
		public string Message { get; set; } = "";
		public T? Data { get; set; }
		public IEnumerable<string> Errors { get; set; } = new List<string>();

		public static ApiResponseDto<T> SuccessResult(T data, string message = "Success")
		{
			return new ApiResponseDto<T>
			{
				Success = true,
				Message = message,
				Data = data
			};
		}

		public static ApiResponseDto<T> FailureResult(string message, IEnumerable<string>? errors = null)
		{
			return new ApiResponseDto<T>
			{
				Success = false,
				Message = message,
				Errors = errors ?? new List<string>()
			};
		}
	}
}
