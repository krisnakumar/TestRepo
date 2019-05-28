namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScheduledReportAudit")]
    public partial class ScheduledReportAudit
    {
        public int Id { get; set; }

        public int ScheduledReportId { get; set; }

        public DateTime DateRan { get; set; }

        public int FileCount { get; set; }

        public int RowCount { get; set; }

        public virtual ScheduledReport ScheduledReport { get; set; }
    }
}
