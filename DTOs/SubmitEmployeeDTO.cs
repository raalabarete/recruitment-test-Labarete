using InterviewTest.Model;

namespace InterviewTest.DTOs
{
    public class SubmitEmployeeDTO : Employee
    {
        public string? prevName {  get; set; }
        public int prevVal { get; set; }
    }
}
