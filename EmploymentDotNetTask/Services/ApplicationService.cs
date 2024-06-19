using EmploymentDotNetTask.Data;
using EmploymentDotNetTask.Dtos;
using EmploymentDotNetTask.FixedValue;
using EmploymentDotNetTask.Helpers.ImplementSection;
using EmploymentDotNetTask.Helpers.Interface;
using EmploymentDotNetTask.IServices;
using EmploymentDotNetTask.Models;

namespace EmploymentDotNetTask.Services
{
	public class ApplicationService : IApplicationService
	{
		private readonly ILogHelper _logger;
		private string classname = nameof(ApplicationService);
		private EmploymentDbContext _db;
		public ApplicationService(ILogHelper logger, EmploymentDbContext db)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<BaseResponseDto<string>> Add(BaseRequestDto<ApplicantRequestDto> input)
		{
			var methodName = $" {classname}/{nameof(Add)}";
			var output = new BaseResponseDto<string>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{
				var applicant = ApplicantRequestDtoToApplicant(input.Request);
				var checkIfExist = _db.Applicants.Where(x => x.Email.ToLower() == applicant.Email.ToLower() || x.Phone == applicant.Phone).FirstOrDefault();
				if (checkIfExist != null && checkIfExist.Email.ToLower() == applicant.Email.ToLower())
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = GeneralResponse.failureMessage;
					output.Response = "Email already exist";
					return output;
				}
				if (checkIfExist != null && checkIfExist.Phone == applicant.Phone)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = GeneralResponse.failureMessage;
					output.Response = "Phone Number already exist";
					return output;
				}
				_db.Applicants.Add(applicant);
				output.ResponseCode = GeneralResponse.sucessCode;
				output.ResponseMessage = GeneralResponse.sucessMessage;
				output.Response = "Applicant saved succesfully";
				_db.SaveChanges();

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = "Applicant not saved succesfully";
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

				var applicant = _db.Applicants.Where(m => m.Id == input.Request.Id).FirstOrDefault();
				if (applicant is null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Applicant does not exist";
					output.Response = new BoolPayload()
					{
						IsTrue = false
					};
					return output;
				}
				else
				{
					_db.Applicants.Remove(applicant);
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

		public async Task<BaseResponseDto<List<ApplicantRequestDto>>> GetAll(BaseRequestDto<string> input)
		{
			var methodName = $" {classname}/{nameof(GetAll)}";
			var output = new BaseResponseDto<List<ApplicantRequestDto>>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{

				var applicants = _db.Applicants.ToList();

				if (applicants.Any())
				{
					output.Response = ListApplicantToApplicantRequestDto(applicants.ToList());
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = GeneralResponse.sucessMessage;
				}
				else
				{
					output.Response = new List<ApplicantRequestDto>();
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = "There are no available applicant";
				}



			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = new List<ApplicantRequestDto>();
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<Application>> GetSingle(BaseRequestDto<IdPayload> input)
		{
			var methodName = $" {classname}/{nameof(GetSingle)}";
			var output = new BaseResponseDto<Application>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{

				var applicant = _db.Applicants.Where(m => m.Id == input.Request.Id).FirstOrDefault();
				if (applicant is null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Applicant does not exist";
					output.Response = new Application();
					return output;
				}
				else
				{
					output.Response = applicant;
					output.ResponseCode = GeneralResponse.sucessCode;
					output.ResponseMessage = GeneralResponse.sucessMessage;
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(input.RequestId, $"Failed", input.Ip, methodName, ex);
				output.ResponseCode = GeneralResponse.failureCode;
				output.ResponseMessage = GeneralResponse.failureMessage;
				output.Response = new Application();
			}
			_logger.LogInformation(input.RequestId, $"Response:{output}", input.Ip, methodName);
			return output;
		}

		public async Task<BaseResponseDto<BoolPayload>> Update(BaseRequestDto<ApplicantRequestDto> input, string applicantId)
		{
			var methodName = $" {classname}/{nameof(Update)}";
			var output = new BaseResponseDto<BoolPayload>();
			_logger.LogInformation(input.RequestId, $"New", input.Ip, methodName);
			try
			{
				var applicant = _db.Applicants.Where(m => m.Id == applicantId).FirstOrDefault();
				if (applicant != null)
				{
					output.ResponseCode = GeneralResponse.failureCode;
					output.ResponseMessage = "Applicant does not exist";
					output.Response.IsTrue = false;
					return output;
				}

				applicant = ApplicantRequestDtoToApplicant(input.Request);

				_db.Applicants.Update(applicant);
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



		private Application ApplicantRequestDtoToApplicant(ApplicantRequestDto input)
		{
			return new Application()
			{
				Id = Guid.NewGuid().ToString(),
				FirstName = input.FirstName,
				LastName = input.LastName,
				Email = input.Email,
				Phone = input.Phone,
				IsPhoneInternal = input.IsPhoneInternal,
				IsCurrentResidenceHidden = input.IsCurrentResidenceHidden,
				IsCurrentResidenceInternal = input.IsCurrentResidenceInternal,
				IsDateOfBirthHidden = input.IsDateOfBirthHidden,
				IsDateOfBirthInternal = input.IsDateOfBirthInternal,
				IsIDNumberHidden = input.IsIDNumberHidden,
				IsIDNumberInternal = input.IsIDNumberInternal,
				IsNationalityHidden = input.IsNationalityHidden,
				IsNationalityInternal = input.IsNationalityInternal,
				IsPhoneHidden = input.IsPhoneHidden,
				Nationality = input.Nationality,
				CurrentResidence = input.CurrentResidence,
				IDNumber = input.IDNumber,
				DateOfBirth = GeneralHelper.StringToDate(input.DateOfBirth),

			};
		}

		private ApplicantRequestDto ApplicantToApplicantRequestDto(Application input)
		{
			return new ApplicantRequestDto()
			{
				Id = input.Id,
				FirstName = input.FirstName,
				LastName = input.LastName,
				Email = input.Email,
				Phone = input.Phone,
				IsPhoneInternal = input.IsPhoneInternal,
				IsCurrentResidenceHidden = input.IsCurrentResidenceHidden,
				IsCurrentResidenceInternal = input.IsCurrentResidenceInternal,
				IsDateOfBirthHidden = input.IsDateOfBirthHidden,
				IsDateOfBirthInternal = input.IsDateOfBirthInternal,
				IsIDNumberHidden = input.IsIDNumberHidden,
				IsIDNumberInternal = input.IsIDNumberInternal,
				IsNationalityHidden = input.IsNationalityHidden,
				IsNationalityInternal = input.IsNationalityInternal,
				IsPhoneHidden = input.IsPhoneHidden,
				Nationality = input.Nationality,
				CurrentResidence = input.CurrentResidence,
				IDNumber = input.IDNumber,
				DateOfBirth = GeneralHelper.DateTimeToString(input.DateOfBirth),

			};
		}


		private List<ApplicantRequestDto> ListApplicantToApplicantRequestDto(List<Application> input)
		{
			var output = new List<ApplicantRequestDto>();
			foreach (var item in input)
			{
				output.Add(ApplicantToApplicantRequestDto(item));
			}

			return output;
		}
	}
}
