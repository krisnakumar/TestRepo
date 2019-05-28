namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("ProductCourse")]
    public partial class ProductCourse
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CourseId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ProductPrice { get; set; }

        public virtual Product Product { get; set; }
    }
}
