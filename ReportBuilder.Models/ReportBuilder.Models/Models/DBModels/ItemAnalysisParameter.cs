using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class ItemAnalysisParameter
    {
        public string companies { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        
       
    }
}