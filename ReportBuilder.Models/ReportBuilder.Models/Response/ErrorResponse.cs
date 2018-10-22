using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Response
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
