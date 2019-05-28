using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Assignments = new List<AssignmentDetail>();
        }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public int TaskVersionId { get; set; }
        public int Task_Id { get; set; }
        public bool IsTaskVersionCurrent { get; set; }
        public string ExpirationDate { get; set; }
        public string Status { get; set; }
        public string SeriesName { get; set; }
        public int SeriesId { get; set; }
        public List<AssignmentDetail> Assignments { get; set; }
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }

    }
}