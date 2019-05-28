using System;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing user details for users in a given company.  Created to mimic users.aspx page for By User coaching report
    /// </summary>
    public class CompanyUserDetails
    {
        [Key]
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Full_Name_Format1 { get; set; }
        public string Company_Name { get; set; }
        public string User_Email { get; set; }
        public bool Is_Default { get; set; }
        public bool Is_Visible { get; set; }
        public string Remote_Id { get; set; }
        public string Supervisors { get; set; }
        public string Role_Name { get; set; }
        public string Department { get; set; }
        public string Job_Title { get; set; }
        public string User_Series { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Full_Name { get; set; }
        public bool IsActiveAdmin { get; set; }
        public DateTime DateAdminActivated { get; set; }
        public string Alternate_User_Name { get; set; }
        public string User_Company_Status { get; set; }
        public string User_Company_Enabled { get; set; }
    }
}