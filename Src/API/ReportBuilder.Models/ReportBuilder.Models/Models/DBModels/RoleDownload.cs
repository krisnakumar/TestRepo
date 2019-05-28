using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class RoleDownload
    {
        public RoleDownload()
        {
            Tasks = new List<string>();
            Courses = new List<string>();
            Users = new List<RoleUser>();
        }
        public string Name { get; set; }
        public List<string> Tasks {get;set; }
        public List<string> Courses { get; set; }
        public List<RoleUser> Users { get; set; }
    }
    public class RoleUser
    {
        public RoleUser() { }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

}