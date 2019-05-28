using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{

    [Table("Acknowledgement")]
    public partial class Acknowledgement
    {
        [Required]
        public int      Id                  { get; set; }

        [Required]
        [StringLength(48)]
        public string   CompanyCode         { get; set; }

        [Required]
        public string   AcknowledgementText { get; set; }

        [Required]
        public string   ModalTitle          { get; set; }

        [Required]
        public int      Options             { get; set; }

        [Required]
        public bool     IsEnabled           { get; set; }
    }
}