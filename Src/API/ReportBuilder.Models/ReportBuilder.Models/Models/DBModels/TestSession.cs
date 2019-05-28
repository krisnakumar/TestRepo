namespace OnBoardLMS.WebAPI.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("TestSession")]
    public partial class TestSession
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string AppointmentId { get; set; }

        [StringLength(64)]
        public string SiteId { get; set; }

        public int? SeatTime { get; set; }

        public int? AllottedTime { get; set; }

        [JsonIgnore]
        [StringLength(40)]
        public string IPAddress { get; set; }

        [JsonIgnore]
        [StringLength(1024)]
        public string UserAgent { get; set; }

        public DateTime SessionStarted { get; set; }

        [NotMapped]
        public int? CourseAssignmentId { get; set; }

        public int? UserId { get; set; }

        [NotMapped]
        public string PartnerId { get; set; }

        public int? CompanyId { get; set; }
    }
}
