namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("SkillProcedure")]
    public partial class SkillProcedure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SkillProcedure()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int SkillId { get; set; }

        public bool IsEnabled { get; set; }

        public bool? IsCustom { get; set; }

        public int? ParentSkillProcedureId { get; set; }

        public string Photo { get; set; }

        public int? DomainId { get; set; }

    }
}
