using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class EWalletSeriesDetail
    {
        [Key]
        public int Series_Id { get; set; }
        public string Series_Name { get; set; }
        public int Use_Role { get; set; }
        public string SeriesValue { get; set; }
    }
}