using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    

    [Table("User")]
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string UserName2 { get; set; }

        [Required]
        [StringLength(512)]
        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(100)]
        public string FName { get; set; }

        [StringLength(100)]
        public string MName { get; set; }

        [Required]
        [StringLength(100)]
        public string LName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public short? PrefAudio { get; set; }

        [StringLength(255)]
        public string PrefLanguage { get; set; }

        public short? PrefSpeed { get; set; }

        public short? PrefText { get; set; }

        public int UserPerms { get; set; }

        public int SettingsPerms { get; set; }

        public int CoursePerms { get; set; }

        public int TranscriptPerms { get; set; }

        public int CompanyPerms { get; set; }

        public int ForumPerms { get; set; }

        public int ComPerms { get; set; }

        public long ReportsPerms { get; set; }

        public int AnnouncementPerms { get; set; }

        public int SystemPerms { get; set; }

        [StringLength(128)]
        public string RemoteId { get; set; }

        public int Preference { get; set; }

        [StringLength(255)]
        public string Street1 { get; set; }

        [StringLength(255)]
        public string Street2 { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Phone2 { get; set; }

        [StringLength(41)]
        public string Photo { get; set; }

        public Guid? UniqueId { get; set; }

        [StringLength(40)]
        public string BarCodePath { get; set; }

        [StringLength(300)]
        public string BarCodeHash { get; set; }

        [StringLength(50)]
        public string ISNetworldId { get; set; }

        public Int16 LoginAttempts { get; set; }

        [StringLength(255)]
        public string PIIFields { get; set; }
    }
}
