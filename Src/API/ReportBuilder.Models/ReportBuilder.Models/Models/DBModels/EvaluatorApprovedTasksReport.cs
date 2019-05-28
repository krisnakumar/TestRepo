using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing data on Evaluator Approved Task report
    /// </summary>
    public partial class EvaluatorApprovedTasksReport
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        [Key, Column(Order=3)]
        public string Series { get; set; }
        [Key, Column(Order = 2)]
        public string Skill_Code { get; set; }
        public string Skill_Name { get; set; }
        public string Departments { get; set; }
        public string Cost_Centers { get; set; }
        public string Work_Locations { get; set; }
        [Key, Column(Order = 1)]
        public int User_Id { get; set; }
    }
}