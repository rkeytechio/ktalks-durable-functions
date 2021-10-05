namespace KTalks.EmployeeOnboarding.Functions.Models.Responses
{
    public class EmployeeDetail
    {
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsExternalContractor => EmployeeId.StartsWith("8");
    }
}
