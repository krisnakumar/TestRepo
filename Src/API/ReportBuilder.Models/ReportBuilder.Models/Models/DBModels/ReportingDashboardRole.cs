using System.ComponentModel.DataAnnotations.Schema;


namespace ReportBuilder.Models.Models.DBModels
{
    [Table("Reporting_DashboardRole")]
    public partial class ReportingDashboardRole
    {
        public int RoleId { get; set; }

        public int DashboardID { get; set; }
    }
}
