using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnBoardLMS.WebAPI.Models
{
    public partial class UserPass
    {
        public UserPass()
        {

        }
        [StringLength(512)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [Key]
        [JsonProperty(PropertyName = "userid")]
        public int UserId { get; set; }
    }
}