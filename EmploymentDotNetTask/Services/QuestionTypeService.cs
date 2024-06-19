using EmploymentDotNetTask.Data;
using EmploymentDotNetTask.Dtos;
using EmploymentDotNetTask.FixedValue;
using EmploymentDotNetTask.Helpers.Interface;
using EmploymentDotNetTask.IServices;
using EmploymentDotNetTask.Models;

namespace EmploymentDotNetTask.Services
{
	public class QuestionTypeService : IQuestionTypeService
	{

		private readonly ILogHelper _logger;
		private string classname = nameof(QuestionTypeService);
		private EmploymentDbContext _db;
		public QuestionTypeService(ILogHelper logger, EmploymentDbContext db)
		{
			_db = db;
			_logger = logger;
		}


		public async Task<BaseResponseDto<string>> Add(BaseRequestDto<QuestionType> input)
		{
			var methodName = $" {classname}/{nameof(Add)}";
			var output = new BaseResponseDto<string>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{
				
				var applicant = input.Request;
				var checkIfExist = _db.QuestionTypes.Where(x => x.Name.ToLower() == applicant.Name.ToLower()).FirstOrDefault();
				if (checkIfExist != null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = GeneralResponse.failureMessage;
					output.Response = "Question type already exist";
					return output;
				}
				_db.QuestionTypes.Add(applicant);
				output.ResponseCode = GeneralResponse.sucessCode;
				output.ResponseMessage = GeneralResponse.sucessMessage;
				output.Response = "Question type saved succesfully";
				_db.SaveChanges();

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = "Question type not saved succesfully";
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<BoolPayload>> Delete(BaseRequestDto<IdPayload> input)
		{
			var methodName = $" {classname}/{nameof(Delete)}";
			var output = new BaseResponseDto<BoolPayload>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{

				var applicant = _db.QuestionTypes.Where(m => m.Id == input.Request.Id).FirstOrDefault();
				if (applicant is null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Question type does not exist";
					output.Response = new BoolPayload()
					{
						IsTrue = false
					};
					return output;
				}
				else
				{
					_db.QuestionTypes.Remove(applicant);
					output.Response = new BoolPayload()
					{
						IsTrue = true
					};
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = GeneralResponse.sucessMessage;
					_db.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = new BoolPayload()
				{
					IsTrue = false
				};
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<List<QuestionType>>> GetAll(BaseRequestDto<string> input)
		{
			var methodName = $" {classname}/{nameof(GetAll)}";
			var output = new BaseResponseDto<List<QuestionType>>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{

				var program = _db.QuestionTypes.ToList();

				if (program.Any())
				{
					output.Response = program.ToList();
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = GeneralResponse.sucessMessage;
				}
				else
				{
					output.Response = new List<QuestionType>();
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = "There are no available question type";
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = new List<QuestionType>();
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<QuestionType>> GetSingle(BaseRequestDto<IdPayload> input)
		{
			var methodName = $" {classname}/{nameof(GetSingle)}";
			var output = new BaseResponseDto<QuestionType>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{

				var program = _db.QuestionTypes.Where(m => m.Id == input.Request.Id).FirstOrDefault();
				if (program is null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Question type does not exist";
					output.Response = new QuestionType();
					return output;
				}
				else
				{
					output.Response = program;
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = GeneralResponse.sucessMessage;
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = new QuestionType();
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<BoolPayload>> Update(BaseRequestDto<QuestionType> input)
		{
			var methodName = $" {classname}/{nameof(Update)}";
			var output = new BaseResponseDto<BoolPayload>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{
				var dbData = _db.QuestionTypes.Where(m => m.Id == input.Request.Id).FirstOrDefault();
				if (dbData != null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Question type does not exist";
					output.Response.IsTrue = false;
					return output;
				}
				dbData = input.Request;



				_db.QuestionTypes.Update(dbData);
				output.ResponseCode = GeneralResponse.sucessCode;
				output.ResponseMessage = GeneralResponse.sucessMessage;
				output.Response.IsTrue = true;
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response.IsTrue = false;
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}
	}
}
