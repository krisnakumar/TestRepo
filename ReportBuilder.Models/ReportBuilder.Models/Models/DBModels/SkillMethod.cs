namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("SkillMethod")]
    public partial class SkillMethod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(12)]
        public string Method { get; set; }
    }
}
