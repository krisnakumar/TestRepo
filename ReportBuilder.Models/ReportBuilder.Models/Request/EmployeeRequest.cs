using ReportBuilder.Models.Models;
using System.Collections.Generic;

namespace ReportBuilder.Models.Request
{
    public class EmployeeRequest
    {
        public List<EmployeeModel> Fields { get; set; }
        public string ColumnList { get; set; }

    }
}
