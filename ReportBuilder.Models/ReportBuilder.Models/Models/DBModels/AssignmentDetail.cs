using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class AssignmentDetail
    {
        public int Task_Id { get; set; }
        public string Task_Name { get; set; }
        public Guid Course_Id { get; set; }
        public int Assignment_Id { get; set; }
        public string Course_Name { get; set; }
        public string Location { get; set; }
        public string Last_Accessed { get; set; }
        public string Status { get; set; }
        public string Progress { get; set; }
        public int Status_Code { get; set; }
        public DateTime? Last_Accessed_Date { get; set; }
        public DateTime? Start_Date { get; set; }
        public int Ready_Status { get; set; }
        public Int16 Options { get; set; }
        public string Expired_Date { get; set; }
        public byte? Display_Type_Allowed { get; set; }
        public long? Course_Options { get; set; }
        public long? CS_Options { get; set; }
        public int User_Id { get; set; }
        public int CA_SubType { get; set; }
        public byte? CC_Options { get; set; }
        public int? CC_EntityId { get; set; }
        public int Lesson_Id { get; set; }
        public int? Lesson_Options { get; set; }
        public int? EstimatedTime { get; set; }
        public int? TestSessionId { get; set; }
        public int? LockoutReason { get; set; }
        public DateTime? NextAvailableDate { get; set; }
        public bool Is_Current { get; set; }
        public string LockoutReasonDesc { get; set; }
        public int NbrOfQuestions { get; set; }
        public string Skill_Code { get; set; }
        public string Skill_Name { get; set; }
        public int? Skill_Type { get; set; }
        public DateTime? Skill_Date_Taken { get; set; }
        public int? Skill_Status_Code { get; set; }
        public DateTime? Skill_NextAvailableDate { get; set; }
        public int? IsRequired { get; set; }
        public int? Procedure_Count { get; set; }
        public DateTime? Skill_Expiration_Date { get; set; }
        public AssignmentDetail ShallowClone()
        {
            return (AssignmentDetail)this.MemberwiseClone();
        }
        public int? SeriesId { get; set; }
    }
}