using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class Element
    {
        public int id { get; set; }
        public string value { get; set; }
        public int sectionId { get; set; }
        public int? exceptionTypeId { get; set; }
        public string exceptionText { get; set; }
        public bool isResolved { get; set; }
        public int? resolvedBy { get; set; }
        public int? resolutionTypeId { get; set; }
        public string resolutionText { get; set; }
        public List<string> images { get; set; }
    }
}