namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScheduledReport")]
    public partial class ScheduledReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScheduledReport()
        {
            ScheduledReportDetails = new HashSet<ScheduledReportDetail>();
        }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public int TimeToSend { get; set; }

        public int RepeatInterval { get; set; }

        public int RepeatIntervalType { get; set; }

        public short ReportId { get; set; }

        public int CompanyId { get; set; }

        public DateTime? LastRunDate { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? DateToRun { get; set; }

        public int? EndCondition { get; set; }

        public DateTime? EndDate { get; set; }

        public int? EndOccurrenceCount { get; set; }

        public int RepeatOn { get; set; }

        [StringLength(3)]
        public string Timezone { get; set; }

        public virtual ICollection<ScheduledReportDetail> ScheduledReportDetails { get; set; }

    }
}
