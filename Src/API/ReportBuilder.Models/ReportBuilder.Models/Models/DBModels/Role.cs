using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{


    [Table("Role")]
    public partial class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Desc { get; set; }

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

        public int CompanyId { get; set; }

        public bool IsShared { get; set; }

        public int? SupervisorId { get; set; }

        public int? SupervisorId2 { get; set; }

        public int? RoleRank { get; set; }

        [StringLength(256)]
        public string CustomScript { get; set; }

        [StringLength(1024)]
        public string CustomScriptDesc { get; set; }

        public bool IsAdmin { get; set; }

        public bool? IsEnabled { get; set; }

        public bool? IsLocked { get; set; }
    }
}
