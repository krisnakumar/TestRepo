using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class ShareLink
    {
        [Key]
        [StringLength(2000)]
        public string Link { get; set; }
    }
}