namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("TaskRoleAssociation")]
    public partial class TaskRoleAssociation
    {
        public int Id { get; set; }

        public int TaskRoleId { get; set; }

        public int AssociatedTaskId { get; set; }

        public short? IsRequired { get; set; }

        public bool IsEnabled { get; set; }

        public int AssociatedTaskRoleId { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeleteReason { get; set; }

        public int? Options { get; set; }
    }
}
