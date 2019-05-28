using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    [Table("DomainRecommendation")]
    public class DomainRecommendation
    {
        [Key, Column("DomainId", Order = 0)]
        public int DomainId { get; set; }

        public String RecommendationText { get; set; }

        [Key, Column("Attempt", Order = 1)]
        public int Attempt { get; set; }

        [Key, Column("EntityId", Order = 2)]
        public int? EntityId { get; set; }

        [Key, Column("EntityType", Order = 3)]
        public short? EntityType { get; set; }

        [Key, Column("IsEnabled", Order = 4)]
        public bool? IsEnabled { get; set; }

    }
}