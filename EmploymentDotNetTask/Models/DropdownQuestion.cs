namespace EmploymentDotNetTask.Models
{
	public class DropdownQuestion : BaseQuestionDbModel
	{
		public List<QuestionItem> QuestionOptions { get; set; }

		public QuestionItem? Answer { get; set; }
	}
}
