using System.Collections.Generic;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class to help with Workbook reporting, details on workbook, user, and company
    /// </summary>
    public class WorkbookReportHeader
    {
        public WorkbookReportHeader()
        {
            Entities = new List<WorkbookReportEntity>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string WorkbookName { get; set; }
        public string WorkbookStatus { get; set; }
        public int WorkbookId { get; set; }
        public List<WorkbookReportEntity> Entities { get; set; }
    }
}