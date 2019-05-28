using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class AssignmentParameter
    {
        public int? Status { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? OpenBook { get; set; }
        public DateTime? DateTaken { get; set; }
        public string AppointmentId { get; set; }
        public string CompanyCode { get; set; }

        /// <summary>
        /// Depending on the type will cause the query to filter the records returned
        /// If this is a prometric request type the query will return the previously failed record if the current record is locked out
        /// </summary>
        public int? RequestType { get; set; }
        public AssignmentParameter()
        {

        }
        public AssignmentParameter(int? status, bool? isCurrent, bool? openBook, DateTime? dateTaken, string appointmentId, int? requestType, string companyCode)
        {
            Status = status;
            IsCurrent = isCurrent;
            OpenBook = openBook;
            DateTaken = dateTaken;
            AppointmentId = appointmentId;
            RequestType = requestType;
            CompanyCode = companyCode;
        }
    }
}