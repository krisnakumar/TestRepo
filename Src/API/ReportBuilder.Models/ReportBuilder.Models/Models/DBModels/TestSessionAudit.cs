using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{

    [Table("TestSessionAudit")]
    public partial class TestSessionAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TestSessionId { get; set; }

        [StringLength(256)]
        public string Event { get; set; }

        public DateTime? DateCreated { get; set; }

        [NotMapped]
        public string PartnerId { get; set; }
    }
}
