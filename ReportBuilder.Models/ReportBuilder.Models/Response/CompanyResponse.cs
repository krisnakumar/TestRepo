using ReportBuilder.Models.Models;
using System.Collections.Generic;

namespace ReportBuilder.Models.Response
{
    public class CompanyResponse
    {
        public List<CompanyModels> Companies { get; set; }
        public ErrorResponse Error { get; set; }

    }
}
