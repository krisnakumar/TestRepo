namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("TaskRole")]
    public partial class TaskRole
    {
        public int TaskId { get; set; }

        public int RoleId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateCreated { get; set; }

        public bool IsEnabled { get; set; }

        public int? Options { get; set; }

        public int Id { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeleteReason { get; set; }
    }
}
