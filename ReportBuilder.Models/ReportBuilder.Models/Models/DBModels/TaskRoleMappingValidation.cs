using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace OnBoardLMS.WebAPI.Models
{
    public class TaskRoleMappingValidation
    {
        [Key]
        public int TaskRoleId { get; set; }
        public int MappingStatus { get; set; }
    }
}