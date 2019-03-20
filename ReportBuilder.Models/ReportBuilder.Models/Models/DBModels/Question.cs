namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }

        public int? QuestionId { get; set; }

        public int LessonId { get; set; }

        [StringLength(256)]
        public string Identifier { get; set; }

        [StringLength(2048)]
        public string Text { get; set; }

        [StringLength(2)]
        public string CorrectAnswerLetter { get; set; }

        public byte? TYPE { get; set; }

        [StringLength(255)]
        public string IncorrectFeedback { get; set; }

        [StringLength(255)]
        public string CorrectFeedback { get; set; }

        [StringLength(1024)]
        public string DomainGroup { get; set; }

        [StringLength(1024)]
        public string ElementGroup { get; set; }

        public int? QuestionRank { get; set; }

        [StringLength(1024)]
        public string QuestionImage { get; set; }

        public bool IsEnabled { get; set; }

        public int? DomainId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
