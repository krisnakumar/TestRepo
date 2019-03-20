using System;


namespace OnBoardLMS.WebAPI.Models
{
    public class UserCompanySeriesDetail
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int SeriesId { get; set; }
        public int Options { get; set; }
        public int Status { get; set; }
        public int LicenseType { get; set; }
        public DateTime? DateActivated { get; set; }

        public DateTime? DateDeActivated { get; set; }

        public int? ActivatedBy { get; set; }

        public int? DeactivatedBy { get; set; }
        public string SeriesName { get; set; }
    }
}