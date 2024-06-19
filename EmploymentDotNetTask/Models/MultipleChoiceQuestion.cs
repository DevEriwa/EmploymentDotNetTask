namespace EmploymentDotNetTask.Models
{
	public class MultipleChoiceQuestion : BaseQuestionDbModel
	{
		public List<QuestionItem> QuestionOptions { get; set; }

		public List<QuestionItem>? QuestionAnswers { get; set; }
	}
}
