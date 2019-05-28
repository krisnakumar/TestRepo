using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class ItemAnalysisSummaryDetail
    {
        /// <summary>
        /// TaskVersion Id
        /// </summary>
        public int TaskVersionId { get; set; }

        /// <summary>
        /// Task Id
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// Task Code
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        /// TaskVersion Name
        /// </summary>
        public string TaskVersionName { get; set; }

        /// <summary>
        /// Is TaskVersion Current
        /// </summary>
        public Boolean TaskVersionIsCurrent { get; set; }

        /// <summary>
        /// SkillId or LessonId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Skill or Task code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Skill or Exam name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Skill Type
        /// </summary>
        public int SkillType { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Desc from CourseVersion
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Is CourseVersion Current
        /// </summary>
        public Boolean IsCurrent { get; set; }

        /// <summary>
        /// SkillId or Course Id2
        /// </summary>
        public int ExamId { get; set; }
    }
}