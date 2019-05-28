namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ScheduledReportDetail")]
    public partial class ScheduledReportDetail
    {
        public int Id { get; set; }

        public int ScheduledReportId { get; set; }

        public int? EntityId { get; set; }

        public int Type { get; set; }

        public bool IsEnabled { get; set; }

    }
}
