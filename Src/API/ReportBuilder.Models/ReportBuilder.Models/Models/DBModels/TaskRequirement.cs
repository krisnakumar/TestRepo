using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class TaskRequirement
    {
        public string SeriesName { get; set; }
        public int SeriesId { get; set; }
        public string TaskCode { get; set; }
        public int TaskVersionId { get; set; }
        public byte TaskVersionOptions { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public Guid? CourseId { get; set; }
        public string CourseName { get; set; }
        public string SkillName { get; set; }
        public string SkillCode { get; set; }
        public int SkillOptions { get; set; }
        public int TaskOptions { get; set; }
        public bool TaskVersionIsCurrent { get; set; }
        public int? SkillId { get; set; }
        public int? SkillRequalificationInterval { get; set; }
        public int? AllowRequalificationPeriod { get; set; }
        public int? IsRequired { get; set; }
        public int? SkillSubmitPeriod { get; set; }
        public int? SkillSubmitPeriodBack { get; set; }
        public long CompanySeriesOptions { get; set; }
        public string IsRequiredText { get; set; }
        public int? SkillType { get; set; }
        /// <summary>
        /// dbo.getRecertInterval
        /// </summary>
        public int? RecertInterval { get; set; }
        public int? SecondarySkillSubmitPeriod { get; set; }
        public string IsEnhanced { get; set; }
        public DateTime? DateTaskExpires { get; set; }
    }
}