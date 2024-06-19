using EmploymentDotNetTask.Dtos;
using EmploymentDotNetTask.FixedValue;
using EmploymentDotNetTask.Helpers.ImplementSection;
using EmploymentDotNetTask.Helpers.Interface;
using EmploymentDotNetTask.IServices;
using EmploymentDotNetTask.Models;
using EmploymentDotNetTask.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Xunit;

namespace EmploymentDotNetTask.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly ILogHelper _logger;
		private string classname = nameof(ApplicationService);
		private IApplicationService _dbService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ApplicationController(ILogHelper logger, IApplicationService db, IWebHostEnvironment webHostEnvironment)
		{
			_dbService = db;
			_logger = logger;
			_webHostEnvironment = webHostEnvironment;
		}
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllData()
		{
			var methodName = $" {classname}/{nameof(GetAllData)}";
			var Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			var requestId = GeneralHelper.GetNewRequestId();
			_logger.LogInformation(requestId, "New Process", Ip, methodName);
			try
			{
				var dbResponse = await _dbService.GetAll(new BaseRequestDto<string>()
				{
					Ip = Ip,
					Request = "",
					RequestId = requestId
				});
				if (dbResponse.ResponseCode == GeneralResponse.sucessCode)
				{
					return Ok(dbResponse);
				}
				else
				{
					return BadRequest(dbResponse);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(requestId, $"Failed", Ip, methodName, ex);
				return BadRequest();
			}


		}

		[HttpGet("Get")]
		public async Task<IActionResult> GetSingle(string Id)
		{
			var methodName = $" {classname}/{nameof(GetSingle)}";
			var Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			var requestId = GeneralHelper.GetNewRequestId();
			_logger.LogInformation(requestId, "New Process", Ip, methodName);
			try
			{
				var dbResponse = await _dbService.GetSingle(new BaseRequestDto<IdPayload>()
				{
					Ip = Ip,
					Request = new IdPayload()
					{
						Id = Id
					},
					RequestId = requestId
				});
				_logger.LogInformation(requestId, $"New Process", Ip, methodName);
				if (dbResponse.ResponseCode == GeneralResponse.sucessCode)
				{
					return Ok(dbResponse);
				}
				else
				{
					return BadRequest(dbResponse);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(requestId, $"Failed", Ip, methodName, ex);
				return BadRequest();
			}

		}

		[HttpPost("Add")]
		public async Task<IActionResult> AddSingle([FromBody] ApplicantRequestDto input)
		{
			var methodName = $" {classname}/{nameof(AddSingle)}";
			var Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			var requestId = GeneralHelper.GetNewRequestId();
			_logger.LogInformation(requestId, "New Process", Ip, methodName);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			try
			{
				var dbResponse = await _dbService.Add(new BaseRequestDto<ApplicantRequestDto>()
				{
					Ip = Ip,
					Request = input,
					RequestId = requestId
				});
				if (dbResponse.ResponseCode == GeneralResponse.sucessCode)
				{
					return Ok(dbResponse);
				}
				else
				{
					return BadRequest(dbResponse);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(requestId, $"Failed", Ip, methodName, ex);
				return BadRequest();
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> UpdateSingle([FromBody] ApplicantRequestDto input, [FromForm] string applicantId)
		{
			var methodName = $" {classname}/{nameof(UpdateSingle)}";
			var Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			var requestId = GeneralHelper.GetNewRequestId();
			_logger.LogInformation(requestId, "New Process", Ip, methodName);
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			try
			{
				var dbResponse = await _dbService.Update(new BaseRequestDto<ApplicantRequestDto>()
				{
					Ip = Ip,
					Request = input,
					RequestId = requestId
				},
				applicantId = applicantId);
				if (dbResponse.ResponseCode == GeneralResponse.sucessCode)
				{
					return Ok(dbResponse);
				}
				else
				{
					return BadRequest(dbResponse);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(requestId, $"Failed", Ip, methodName, ex);
				return BadRequest();
			}
		}
		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete(string Id)
		{
			var methodName = $" {classname}/{nameof(Delete)}";
			var Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			var requestId = GeneralHelper.GetNewRequestId();
			_logger.LogInformation(requestId, "New Process", Ip, methodName);
			try
			{
				var dbResponse = await _dbService.Delete(new BaseRequestDto<IdPayload>()
				{
					Ip = Ip,
					Request = new IdPayload()
					{
						Id = Id
					},
					RequestId = requestId
				});
				_logger.LogInformation(requestId, $"New Process", Ip, methodName);
				if (dbResponse.ResponseCode == GeneralResponse.sucessCode)
				{
					return Ok(dbResponse);
				}
				else
				{
					return BadRequest(dbResponse);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(requestId, $"Failed", Ip, methodName, ex);
				return BadRequest();
			}

		}

	}
}
