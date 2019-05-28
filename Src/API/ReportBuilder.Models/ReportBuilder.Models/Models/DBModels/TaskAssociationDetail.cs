using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    

    public partial class TaskAssociationDetail
    {
        [StringLength(1024)]
        public string Task_Name { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Task_Id { get; set; }

        [StringLength(1024)]
        public string AssociatedTaskName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Associated_Task_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Type { get; set; }

        public int? CompanyId { get; set; }

        public int? Options { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid Course_Id { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_IntId { get; set; }
    }
}
