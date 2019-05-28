namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("UserRole")]
    public partial class UserRole
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDefault { get; set; }

        public int? ActivatedBy { get; set; }

        public DateTime? DateActivated { get; set; }

        public int? DeactivatedBy { get; set; }

        public DateTime? DateDeactivated { get; set; }
    }
}
