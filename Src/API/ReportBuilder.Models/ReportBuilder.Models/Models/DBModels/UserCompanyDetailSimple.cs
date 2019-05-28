using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class UserCompanyDetailSimple
    {
        
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Full_Name { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Full_Name_Format1 { get; set; }
        public bool Is_Enabled { get; set; }
        public bool User_Company_Enabled { get; set; }
        public string User_Email { get; set; }
        public int User_Preference { get; set; }
        public string Remote_Id { get; set; }
        public string Alternate_User_Name { get; set; }
        public string Job_Title { get; set; }
        public int Company_Permissions { get; set; }
        public int Course_Permissions { get; set; }
        public int Forum_Permissions { get; set; }
        public int System_Permissions { get; set; }
        public int Settings_Permissions { get; set; }
        public int User_Permissions { get; set; }
        public int Transcript_Permissions { get; set; }
        public int Communication_Permissions { get; set; }
        public long Report_Permissions { get; set; }
        public int Announcement_Permissions { get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public bool Is_Default { get; set; }
        public bool Company_Enabled { get; set; }
        public bool Is_Login_Company { get; set; }
        public byte License_Type { get; set; }
        public byte User_Company_Status { get; set; }

        public int? SupervisorId { get; set; }
        public string SupervisorUserName { get; set; }
        public int? SupervisorId2 { get; set; }
        public string Supervisor2UserName { get; set; }
        public string ISNetworld_Id { get; set; }
        public string Hire_Date { get; set; }
        public bool Is_Visible { get; set; }
        public string Departments { get; set; }
        public string CostCenters { get; set; }
        public string WorkLocations { get; set; }
    }
}