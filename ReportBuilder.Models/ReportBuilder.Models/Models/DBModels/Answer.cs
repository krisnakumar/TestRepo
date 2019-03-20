using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{

    [Table("Answer")]
    public partial class Answer
    {
        public int Id { get; set; }

        [StringLength(2)]
        public string Letter { get; set; }

        [StringLength(1024)]
        public string Text { get; set; }

        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; }

        [StringLength(512)]
        public string AnswerIdentifier { get; set; }

        public bool IsEnabled { get; set; }

        public virtual Question Question { get; set; }
    }
}
