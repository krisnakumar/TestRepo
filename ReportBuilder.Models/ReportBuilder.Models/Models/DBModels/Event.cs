namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Event")]
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            EventCourses = new HashSet<EventCourse>();
            EventOrders = new HashSet<EventOrder>();
            EventRosters = new HashSet<EventRoster>();
        }

        public int Id { get; set; }

        [StringLength(2048)]
        public string Title { get; set; }

        [StringLength(8000)]
        public string Description { get; set; }

        public bool? IsEnabled { get; set; }

        public int? Capacity { get; set; }

        [StringLength(8000)]
        public string Location { get; set; }

        public int? OrganizerId { get; set; }

        public int? EvaluatorId { get; set; }

        public int? FacilitatorId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? StartDateTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? StopDateTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCreated { get; set; }

        public int? CompanyId { get; set; }

        public short? Type { get; set; }

        public short? Status { get; set; }

        [StringLength(3)]
        public string TimeZone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventCourse> EventCourses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventOrder> EventOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventRoster> EventRosters { get; set; }
    }
}
