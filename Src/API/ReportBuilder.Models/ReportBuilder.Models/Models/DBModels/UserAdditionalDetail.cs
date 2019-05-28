using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class UserFullDetail
    {
        public UserFullDetail()
        {

        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string User_Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(302)]
        public string Full_Name { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string Last_Name { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string First_Name { get; set; }

        [StringLength(100)]
        public string Middle_Name { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(303)]
        public string Full_Name_Format1 { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool Is_Enabled { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool User_Company_Enabled { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(100)]
        public string User_Email { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(512)]
        public string User_Password { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Default_Page { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Preference { get; set; }

        [StringLength(128)]
        public string Remote_Id { get; set; }

        [StringLength(100)]
        public string Alternate_User_Name { get; set; }

        [StringLength(128)]
        public string Job_Title { get; set; }

        [Key]
        [Column(Order = 12)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Company_Permissions { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_Permissions { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Forum_Permissions { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int System_Permissions { get; set; }

        [Key]
        [Column(Order = 16)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Settings_Permissions { get; set; }

        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Permissions { get; set; }

        [Key]
        [Column(Order = 18)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Transcript_Permissions { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Communication_Permissions { get; set; }

        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Report_Permissions { get; set; }

        [Key]
        [Column(Order = 21)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Announcement_Permissions { get; set; }

        [Key]
        [Column(Order = 22)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Company_Id { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(255)]
        public string Company_Name { get; set; }

        [Key]
        [Column(Order = 24)]
        public bool Is_Default { get; set; }

        [Key]
        [Column(Order = 25)]
        public bool Company_Enabled { get; set; }

        [Key]
        [Column(Order = 26)]
        public bool Is_Login_Company { get; set; }

        [Key]
        [Column(Order = 27)]
        public byte License_Type { get; set; }

        [Key]
        [Column(Order = 28)]
        public byte User_Company_Status { get; set; }

        public int? SupervisorId { get; set; }
        public string SupervisorFullName { get; set; }
        public string SupervisorUserName { get; set; }

        public int? SupervisorId2 { get; set; }
        public string Supervisor2FullName { get; set; }
        public string Supervisor2UserName { get; set; }

        [StringLength(50)]
        public string ISNetworld_Id { get; set; }

        [StringLength(30)]
        public string Hire_Date { get; set; }

        [Key]
        [Column(Order = 29)]
        public bool Is_Visible { get; set; }

        public string CostCenters { get; set; }
        public string Departments { get; set; }
        public string WorkLocations { get; set; }
    }
}