
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    
    public partial class TaskRoleAssociationDetail
    {
        [Key]
        public int TaskRoleAssociationId { get; set;}
        public int TaskRoleId { get; set; }
        public int AssociatedTaskId { get; set; }

        [StringLength(20)]
        public string IsRequiredText { get; set; }
        [StringLength(128)]
        public string TaskCode { get; set; }
        [StringLength(512)]
        public string TaskName { get; set; }

        public short? IsRequired { get; set; }
        public int? AssociatedTaskRoleId { get; set; }

        public int? Options { get; set; }
    }
}