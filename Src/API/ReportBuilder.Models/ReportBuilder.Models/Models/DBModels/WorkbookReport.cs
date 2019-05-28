using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing data on Workbook reports
    /// </summary>
    public partial class WorkbookReport
    {
        [Key, Column(Order = 1)]
        public int User_Id { get; set; }
        [Key, Column(Order = 2)]
        public int Workbook_Id { get; set; }
        [Key, Column(Order = 3)]
        public int Entity_Id { get; set; }
        public string Workbook_Name { get; set; }
        public string User_Name { get; set; }
        public string User_Full_Name { get; set; }
        public string Company_Name { get; set; }
        public string Workbook_Status { get; set; } //would be status in that workbook?  Completed/Not Complete?
        public string Workbook_Entity_Status { get; set; } //would be status in that specific workbook entity?  Completed/Not Complete?
        public string Entity_Code { get; set; }
        public string Entity_Name { get; set; }
        public int Entity_Type { get; set; }
        public string Last_Attempt_Date { get; set; }
        public string First_Attempt_Date { get; set; }
        public int? Last_Sign_Off_By_Id { get; set; }
        public string Last_Sign_Off_By_Name { get; set; }
        public int Repetitions { get; set; }
        public int Number_Completed { get; set; }
        public int Remaining_Reps { get; set; }
    }
}