using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public partial class NotificationDetail
    {
        [Key]
        //Unique Id based on notification's type.  Ex:  NOC notification would be the TaskRevisionId
        public int EntityId { get; set; }

        //Type of notification (maps to NotificationType enum)
        public int Type { get; set; }

        //User the notification was sent to
        public int UserId { get; set; }

        //User the notification was created by
        public int CreatedByUserId { get; set; }

        //Notification title
        public string Title { get; set; }

        //Date notification was created
        public DateTime DateCreated { get; set; }

        //Date notification expires
        public DateTime? ExpirationDate { get; set; }

        //CompanyId the notification was created by
        public int CreatedByCompanyId { get; set; }

        //Name of company that created the notification
        public string CreatedByCompanyName { get; set; }

        //Company preference of the company that created the notification (helps retrieve their company logo)
        public long CreatedByCompanyPreference { get; set; }

        //URL of the company's logo (custom or default)
        public string CompanyLogo { get; set; }

        //Notification message
        public string Message { get; set; }

        //Notification status
        public string Status { get; set; }

        

        //Full name of user who created the notification
        public string CreatedByFullName { get; set; }

        //URL used for when user selects the notification, will be taken to this address
        public string ActionURL { get; set; }

        //Task Revision Type maps to BusinessObjets.TaskRevisionType enum
        public Int16 TaskRevisionType { get; set; }
    }
}