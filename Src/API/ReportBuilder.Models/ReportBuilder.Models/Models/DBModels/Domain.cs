namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Domain")]
    public partial class Domain
    {
        public int Id { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public bool? IsEnabled { get; set; }

        public short? Type { get; set; }

        public int SeriesId { get; set; }
    }
}
