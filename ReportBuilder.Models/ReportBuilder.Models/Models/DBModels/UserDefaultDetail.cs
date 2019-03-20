using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace OnBoardLMS.WebAPI.Models
{
    public class UserDefaultDetail
    {
        [Key]
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Full_Name { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Full_Name_Format1 { get; set; }
        public bool Is_Enabled  { get; set; }
        public bool User_Company_Enabled { get; set; }
        public string User_Email { get; set; }
        public int User_Preference { get; set; }
        public string Remote_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public bool Is_Default { get; set; }
        public bool Company_Enabled { get; set; }
        public bool Is_Login_Company { get; set; }
        public byte License_Type { get; set; }
        public byte User_Company_Status { get; set; }
        public int? SupervisorId { get; set; }
        public int? SupervisorId2 { get; set; }
        public string ISNetworld_Id { get; set; }
        public DateTime? Hire_Date { get; set; }
        public bool Is_Visible { get; set; }
        public string Alternate_User_Name { get; set; }
				[JsonIgnore]
        public string Password { get; set; }
        public DateTime Date_Created { get; set; }
        public bool User_Is_Enabled { get; set; }
        public Guid? UniqueId { get; set; }
        public string JobTitle { get; set; }
        public string Photo { get; set; }
        public string Supervisor_Name { get; set; }
        public string Phone { get; set; }
        public byte Language { get; set; }
        public long Company_Pref { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Company_Logo { get; set; }
    }
}