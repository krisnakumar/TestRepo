namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Skill")]
    public partial class Skill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Skill()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        [StringLength(48)]
        public string Code { get; set; }

        [StringLength(48)]
        public string B31QCode { get; set; }

        [StringLength(48)]
        public string DOT192Code { get; set; }

        public DateTime DateCreated { get; set; }

        public int SeriesId { get; set; }

        public string AOC { get; set; }

        public int? Options { get; set; }

        public int? RequalificationInterval { get; set; }

        public bool? IsCustom { get; set; }

        public int? ParentSkillId { get; set; }

        public int? Type { get; set; }

        public int? AllowRequalificationPeriod { get; set; }
    }
}
