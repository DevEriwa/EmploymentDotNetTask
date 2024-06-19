using EmploymentDotNetTask.Dtos;
using EmploymentDotNetTask.Models;

namespace EmploymentDotNetTask.IServices
{
	public interface IQuestionTypeService
	{
		Task<BaseResponseDto<string>> Add(BaseRequestDto<QuestionType> input);
		Task<BaseResponseDto<BoolPayload>> Update(BaseRequestDto<QuestionType> input);
		Task<BaseResponseDto<BoolPayload>> Delete(BaseRequestDto<IdPayload> input);
		Task<BaseResponseDto<QuestionType>> GetSingle(BaseRequestDto<IdPayload> input);
		Task<BaseResponseDto<List<QuestionType>>> GetAll(BaseRequestDto<string> input);
	}
}
