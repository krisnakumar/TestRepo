namespace ReportBuilder.Models.Request
{
    public class AuthorizorRequest
    {
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public string UserName { get; set; }
    }
}
