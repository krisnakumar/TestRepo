using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class UserTranscriptDetail
    {
        public string User_Full_Name { get; set; }
        public int? User_Id { get; set; }
        public string User_Name { get; set; }
        public string Task_Code { get; set; }
        public string Company_Name { get; set; }
        public int? Knowledge_Third_Party_Verified { get; set; }
        public string Knowledge_Media_Type { get; set; }
        public string Knowledge_Status { get; set; }
        public byte Knowledge_Status_Code { get; set; }
        public string Skill_Name { get; set; }
        public string Skill_Code { get; set; }
        public string Skill_Affidavit { get; set; }
        public string Skill_Method { get; set; }
        public byte? Skill_Cert_Status { get; set; }
        public DateTime? Date_Skill_Taken { get; set; }
        public DateTime? Date_Skill_Certified { get; set; }
        public DateTime? Date_Skill_Cert_Expired { get; set; }
        public string Skill_Note { get; set; }
        public string Skill_Status { get; set; }
        public byte? Skill_Status_Code { get; set; }
        public string Evaluator_Name { get; set; }
        public int? Evaluator_Id { get; set; }
        public bool? SkillIsCurrent { get; set; }
        public bool? KnowledgeIsCurrent { get; set; }
        public int? CompanyId { get; set; }
        public int? SeriesId { get; set; }
        public int? TaskId { get; set; }
        public int? OrderNumber { get; set; }
        [Key]
        [Column(Order = 0)]
        public int? Course_Access_Id { get; set; }
        public string ConfCode { get; set; }
        public Guid? CourseId { get; set; }
        public int? SkillId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int? SkillActivityId { get; set; }
        public int? TaskVersionId { get; set; }
        public int? Course_Access_Options { get; set; }
        public string Task_Name { get; set; }
        public string Knowledge_Cert_Status { get; set; }
        public DateTime? Date_Knowledge_Taken { get; set; }
        public DateTime? Date_Knowledge_Certified { get; set; }
        public DateTime? Date_Knowledge_Cert_Expired { get; set; }
        public string Knowledge_Note { get; set; }
        public int? KnowledgeAttempt { get; set; }
        public int? SkillAttempt { get; set; }
        public bool? Course_Can_Certify { get; set; }
        public string Proctor_Name { get; set; }
        public int? Proctor_Id { get; set; }
        public string DecertBy_Name { get; set; }
        public int? DecertBy_Id { get; set; }
        public string Decert_Reason_Text { get; set; }
        public int? Decert_Reason_Id { get; set; }
        public DateTime? Decert_Date { get; set; }
        public string Course_Affidavit { get; set; }
        public DateTime? Date_Current_Expired { get; set; }
        [Key]
        [Column(Order = 2)]
        public string Role_Name { get; set; }
        public string Role_Status { get; set; }
        public string Series_Name { get; set; }
        public int? Role_Id { get; set; }
        public int? Use_Role { get; set; }
        public int? On_Task_Profile { get; set; }
        public int? Use_NotQualified { get; set; }
        public int? View_Qualified { get; set; }
        public int? View_NotQualified { get; set; }
        public int? View_All { get; set; }
        public int? View_Skills { get; set; }
        public int? View_Jobs { get; set; }
    }
}
