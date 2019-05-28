using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class FormData
    {
        /// <summary>
        /// Form completed id
        /// </summary>
        public int formVersionId { get; set; }

        /// <summary>
        /// JSON of the form
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public List<Element> elements { get; set; }

        /// <summary>
        /// Auditor id
        /// </summary>
        public int auditorId { get; set; }

        /// <summary>
        /// Student id
        /// </summary>
        public int studentId { get; set; }

        /// <summary>
        /// Date Performed
        /// </summary>
        public System.DateTime datePerformed { get; set; }

        /// <summary>
        /// Time zone
        /// </summary>
        public string timezone { get; set; }

        /// <summary>
        /// Signature Url
        /// </summary>
        public string signatureUrl { get; set; }
    }
}