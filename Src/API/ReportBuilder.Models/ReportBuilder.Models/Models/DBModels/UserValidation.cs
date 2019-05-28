using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    public class UserValidation
    {
        [Key]
        public int UserId { get; set; }

        public int RegistrationStatus { get; set; }

        public string FullName { get; set; }
    }
}