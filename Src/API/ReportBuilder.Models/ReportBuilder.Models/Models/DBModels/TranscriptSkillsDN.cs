using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    public partial class TranscriptSkillsDN 
    {
        [MaxLength(256)]
        public String User_Full_Name { get; set; }
        [Key, Column("User_Id", Order = 0)]
        public Int32 User_Id { get; set; }

        [MaxLength(128)]
        public String User_Name { get; set; }

        [MaxLength(128)]
        public String Task_Code { get; set; }

        [MaxLength(256)]
        public String Company_Name { get; set; }

        public int? Knowledge_Third_Party_Verified { get; set; }

        [MaxLength(32)]
        public String Knowledge_Media_Type { get; set; }

        [MaxLength(32)]
        public String Knowledge_Status { get; set; }

        public Byte? Knowledge_Status_Code { get; set; }

        [MaxLength(1024)]
        public String Skill_Name { get; set; }

        [MaxLength(128)]
        public String Skill_Code { get; set; }

        [MaxLength(256)]
        public String Skill_Affidavit { get; set; }

        [MaxLength(32)]
        public String Skill_Method { get; set; }

        public byte? Skill_Cert_Status { get; set; }
        [Key, Column("Date_Skill_Taken", Order = 6)]
        public DateTime? Date_Skill_Taken { get; set; }

        public DateTime? Date_Skill_Certified { get; set; }

        public DateTime? Date_Skill_Cert_Expired { get; set; }

        [MaxLength(4000)]
        public String Skill_Note { get; set; }

        [MaxLength(32)]
        public String Skill_Status { get; set; }

        public byte? Skill_Status_Code { get; set; }

        [MaxLength(256)]
        public String Evaluator_Name { get; set; }

        public int? Evaluator_Id { get; set; }

        public bool? SkillIsCurrent { get; set; }

        public bool? KnowledgeIsCurrent { get; set; }
        [Key, Column("companyid", Order = 2)]
        public int companyid { get; set; }

        public int? SeriesId { get; set; }

        public int? TaskId { get; set; }

        public int? OrderNumber { get; set; }
        [Key, Column("Course_Access_Id", Order = 1)]
        public int Course_Access_Id { get; set; }

        [MaxLength(40)]
        public String ConfCode { get; set; }

        public Guid? CourseId { get; set; }

        public int? SkillId { get; set; }
        [Key, Column("IsEnabled", Order = 3)]
        public bool IsEnabled { get; set; }
        [Key, Column("SkillActivityId", Order = 4)]
        public int? SkillActivityId { get; set; }

        public int? TaskVersionId { get; set; }

        public int? Course_Access_Options { get; set; }

        [MaxLength(1024)]
        public String Task_Name { get; set; }

        [MaxLength(32)]
        public String Knowledge_Cert_Status { get; set; }

        [Key, Column("Date_Knowledge_Taken", Order = 5)]
        public DateTime? Date_Knowledge_Taken { get; set; }

        public DateTime? Date_Knowledge_Certified { get; set; }

        public DateTime? Date_Knowledge_Cert_Expired { get; set; }

        [MaxLength(512)]
        public String Knowledge_Note { get; set; }

        public int? KnowledgeAttempt { get; set; }

        public int? SkilLAttempt { get; set; }

        public bool? Course_Can_Certify { get; set; }

        [MaxLength(128)]
        public String Proctor_Name { get; set; }

        public int? Proctor_Id { get; set; }

        [MaxLength(128)]
        public String DecertBy_Name { get; set; }

        public int? DecertBy_Id { get; set; }

        [MaxLength(250)]
        public String Decert_Reason_Text { get; set; }

        public int? Decert_Reason_Id { get; set; }

        public DateTime? Decert_Date { get; set; }

        [MaxLength(256)]
        public String Course_Affidavit { get; set; }

        public DateTime? Date_Current_Expired { get; set; }

        public int? Skill_Type { get; set; }
        public string Evaluator_UserName { get; set; }
        public string Proctor_UserName { get; set; }
    }
}
