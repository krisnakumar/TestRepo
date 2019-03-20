using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{

    [Table("UserCompany")]
    public partial class UserCompany
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyId { get; set; }

        public int UserPerms { get; set; }

        public int SettingsPerms { get; set; }

        public int CoursePerms { get; set; }

        public int TranscriptPerms { get; set; }

        public int CompanyPerms { get; set; }

        public int ForumPerms { get; set; }

        public int ComPerms { get; set; }

        public long ReportsPerms { get; set; }

        public int AnnouncementPerms { get; set; }

        public int SystemPerms { get; set; }

        public bool IsDefault { get; set; }

        public bool IsEnabled { get; set; }

        public int DefaultPage { get; set; }

        public bool IsLoginCompany { get; set; }

        public byte LicenseType { get; set; }

        public int? SupervisorId { get; set; }

        public int? SupervisorId2 { get; set; }

        [StringLength(128)]
        public string JobTitle { get; set; }

        public DateTime? DateActivated { get; set; }

        public DateTime? DateDeActivated { get; set; }

        public int? ActivatedBy { get; set; }

        public int? DeactivatedBy { get; set; }

        public byte Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? HireDate { get; set; }

        public bool IsVisible { get; set; }
    }
}
