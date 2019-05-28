using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class DataAuditParameter
    {
        public DataAuditParameter()
        {
            status = -1;
            Default = true;
            visible = true;
            crossCompany = false;
        }
        public int? status { get; set; }
        public bool? Default { get; set; }
        public bool? visible { get; set; }
        public bool? crossCompany { get; set; }
    }
}