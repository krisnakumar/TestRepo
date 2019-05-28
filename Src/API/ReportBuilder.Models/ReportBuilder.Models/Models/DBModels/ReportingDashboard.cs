using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReportBuilder.Models.Models.DBModels
{
    [Table("Reporting_Dashboard")]
    public partial class ReportingDashboard
    {
        public int Id { get; set; }

        public string DashboardName { get; set; }
    }
}
