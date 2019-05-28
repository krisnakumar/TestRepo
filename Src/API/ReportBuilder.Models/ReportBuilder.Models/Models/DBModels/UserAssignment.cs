using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class UserAssignment
    {
        string TaskName { get; set; }
        string ExamName { get; set; }
        Guid CourseId { get; set; }
        string Token { get; set; }
        string LockoutReason { get; set; }
        string Status { get; set; }
        DateTime StartDate { get; set; }
        DateTime ExpirationDate { get; set; }
        bool IsLockedOut { get; set; }

    }
}