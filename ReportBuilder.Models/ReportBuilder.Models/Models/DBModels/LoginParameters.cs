using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class LoginParameters
    {
        public string partId { get; set; }
        public string apptUserId { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
}