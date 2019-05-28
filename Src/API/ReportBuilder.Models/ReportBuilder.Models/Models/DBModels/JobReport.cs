using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Model used for Manage Jobs report
    /// </summary>
    public class JobReport
    {
        public JobReport()
        {
        }

        /// <summary>
        /// UserId
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// User's full name
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Date Qualified
        /// </summary>
        public string DateQualified { get; set; }

        /// <summary>
        /// Date Qualification Expires
        /// </summary>
        public string DateQualExpires { get; set; }

        /// <summary>
        /// Job Status
        /// </summary>
        public string JobStatus { get; set; }

        /// <summary>
        /// Qualification Status
        /// </summary>
        public string QualificationStatus { get; set; }

        /// <summary>
        /// Name of task
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Name of company
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Job Title
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// JobMatrixId
        /// </summary>
        public int JobMatrixId { get; set; }

        /// <summary>
        /// Id of task
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Id of company
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Id of company the job belongs to
        /// </summary>
        public int JobCompanyId { get; set; }

        /// <summary>
        /// Name of company the job belongs to
        /// </summary>
        public string JobCompanyName { get; set; }

        ///// <summary>
        ///// Id of course access record
        ///// </summary>
        //public int? CourseAccessId { get; set; }

        ///// <summary>
        ///// Is Required flag
        ///// </summary>
        //public Int16 IsRequired { get; set; }

        ///// <summary>
        ///// Is Enabled flag
        ///// </summary>
        //public bool IsEnabled { get; set; }
    }
}