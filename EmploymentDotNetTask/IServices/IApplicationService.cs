using EmploymentDotNetTask.Dtos;
using EmploymentDotNetTask.Models;

namespace EmploymentDotNetTask.IServices
{
	public interface IApplicationService
	{
		Task<BaseResponseDto<string>> Add(BaseRequestDto<ApplicantRequestDto> input);
		Task<BaseResponseDto<BoolPayload>> Update(BaseRequestDto<ApplicantRequestDto> input, string applicantId);
		Task<BaseResponseDto<BoolPayload>> Delete(BaseRequestDto<IdPayload> input);
		Task<BaseResponseDto<Application>> GetSingle(BaseRequestDto<IdPayload> input);
		Task<BaseResponseDto<List<ApplicantRequestDto>>> GetAll(BaseRequestDto<string> input);
	}
}
