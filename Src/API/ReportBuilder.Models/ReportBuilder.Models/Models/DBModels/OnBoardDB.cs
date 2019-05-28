//namespace OnBoardLMS.WebAPI.Models
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;
//    using System.Data.SqlClient;

//    public partial class OnBoardDB : DbContext, IOnBoardDB
//    {
  
//        public OnBoardDB()
//            : base("name=connstring")
//        {
//            Database = base.Database;
//        }

//        public virtual DbSet<UserCompany> UserCompanies { get; set; }
//        public virtual DbSet<CoursePreReqDetail> CoursePreReqDetails { get; set; }
//        public virtual DbSet<Event> Events { get; set; }
//        public virtual DbSet<EventCourse> EventCourses { get; set; }
//        public virtual DbSet<EventRoster> EventRosters { get; set; }
//        public virtual DbSet<EventOrder> EventOrders { get; set; }
//        public virtual DbSet<UserCompanyDetail> UserCompanyDetails { get; set; }
//        public virtual DbSet<EvaluatorApprovedTask> EvaluatorApprovedTasks { get; set; }
//        public virtual DbSet<EvaluatorApprovedCourse> EvaluatorApprovedCourses { get; set; }
//        public virtual DbSet<TaskAssociationDetail> TaskAssociationDetails { get; set; }
//        public virtual DbSet<CourseDetail> CourseDetails { get; set; }
//        public virtual DbSet<CourseSkill> CourseSkills { get; set; }
//        public virtual DbSet<EvaluatorApprovedTaskByCourse> EvaluatorApprovedTaskByCourses { get; set; }
//        public virtual DbSet<Question> Questions { get; set; }
//        public virtual DbSet<Answer> Answers { get; set; }
//        public virtual DbSet<CourseAccess> CourseAccesss { get; set; }
//        public virtual DbSet<CompanyCourse> CompanyCourses { get; set; }
//        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
//        public virtual DbSet<Product> Products { get; set; }
//        public virtual DbSet<ProductCourse> ProductCourses { get; set; }
//        public virtual DbSet<CourseAssignment> CourseAssignments { get; set; }
//        public virtual DbSet<User> Users { get; set; }
//        public virtual DbSet<TaskRole> TaskRoles { get; set; }
//        public virtual DbSet<TaskRoleAssociation> TaskRoleAssociations { get; set; }
//        public virtual DbSet<UserCompanySeries> UserCompanySeries { get; set; }
//        public virtual DbSet<Series> Series { get; set; }
//        public virtual DbSet<Form> Forms { get; set; }
//        public virtual DbSet<FormAttachment> FormAttachments { get; set; }
//        public virtual DbSet<FormCompleted> FormCompleteds { get; set; }
//        public virtual DbSet<FormField> FormFields { get; set; }
//        public virtual DbSet<FormSection> FormSections { get; set; }
//        public virtual DbSet<FormTask> FormTasks { get; set; }
//        public virtual DbSet<FormVersion> FormVersions { get; set; }
//        public virtual DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }
//        public virtual DbSet<AssessmentCompleted> AssessmentCompleteds { get; set; }
//        public virtual DbSet<AssessmentException> AssessmentExceptions { get; set; }
//        public virtual DbSet<AssessmentImage> AssessmentImages { get; set; }
//        public virtual DbSet<AssessmentMetric> AssessmentMetrics { get; set; }
//        public virtual DbSet<Task> Tasks { get; set; }
//        public virtual DbSet<DataAudit> DataAudits { get; set; }
//        public virtual DbSet<DataAuditReport> DataAuditReports { get; set; }
//        public virtual DbSet<Company> Companies { get; set; }
//        public virtual DbSet<CompanyClient> CompanyClients { get; set; }
//        public virtual DbSet<TestSession> TestSessions { get; set; }
//        public virtual DbSet<TestSessionAudit> TestSessionAudits { get; set; }
//        public virtual DbSet<CompanyIntegration> CompanyIntegrations { get; set; }
//        public virtual DbSet<SkillActivityDraft> SkillActivityDrafts { get; set; }
//        public virtual DbSet<Skill> Skills { get; set; }
//        public virtual DbSet<CompanySeries> CompanySeriess { get; set; }
//        public virtual DbSet<SkillMethod> SkillMethods { get; set; }
//        public virtual DbSet<Role> Roles { get; set; }
//        public virtual DbSet<UserRole> UserRoles { get; set; }
//        public virtual DbSet<CourseRole> CourseRoles { get; set; }
//        public virtual DbSet<SkillActivity> SkillActivities { get; set; }
//        public virtual DbSet<SkillActivityMetric> SkillActivityMetrics { get; set; }
//        public virtual DbSet<SkillProcedure> SkillProcedures { get; set; }
//        public virtual DbSet<Domain> Domains { get; set; }
//        public virtual DbSet<DomainRecommendation> DomainRecommendations { get; set; }
//        public virtual DbSet<Template> Templates { get; set; }
//        public virtual DbSet<JobMatrix> JobMatrices { get; set; }
//        public virtual DbSet<JobMatrixTask> JobMatrixTasks { get; set; }
//        public virtual DbSet<UserCompanyTransfer> UserCompanyTransfers { get; set; }
//        public virtual DbSet<Acknowledgement> Acknowledgements { get; set; }
//        public virtual DbSet<CompanyCodeCourseMapping> CompanyCodeCourseMappings { get; set; }
//        public virtual DbSet<PIIField> PIIFields { get; set; }
//        public virtual DbSet<PIICompanyField> PIICompanyFields { get; set; }
//        public virtual DbSet<EvaluatorApproval> EvaluatorApprovals { get; set; }
//        public virtual DbSet<Reference> References { get; set; }
//        public virtual DbSet<ApprovalReference> ApprovalReferences { get; set; }
//        public virtual DbSet<Approval> Approvals { get; set; }
//        public virtual DbSet<ApprovalDenialReason> ApprovalDenialReasons { get; set; }
//        public virtual DbSet<FailedSkillActivity> FailedSkillActivities { get; set; }
//        public virtual DbSet<Workbook> Workbooks { get; set; }
//        public virtual DbSet<WorkbookContent> WorkbookContents { get; set; }
//        public virtual DbSet<UserWorkbook> UserWorkbooks { get; set; }
//        public virtual DbSet<WorkbookProgress> WorkbookProgresses { get; set; }
//        public virtual DbSet<WorkbookAudit> WorkbookAudits { get; set; }
//        public virtual DbSet<CourseAttachment> CourseAttachments { get; set; }
//        public virtual DbSet<ScheduledReport> ScheduledReports { get; set; }
//        public virtual DbSet<ScheduledReportDetail> ScheduledReportDetails { get; set; }
//        public virtual DbSet<ScheduledReportAudit> ScheduledReportAudits { get; set; }
//        public virtual DbSet<RestrictedUser> RestrictedUsers { get; set; }

//        public void SetModified(object entity)
//        {
//            Entry(entity).State = EntityState.Modified;
//        }

//        public void SetAdded(object entity)
//        {
//            Entry(entity).State = EntityState.Added;
//        }
//        /// <summary>
//        /// Executes the sql query given returning the number of rows affected
//        /// </summary>
//        /// <param name="sqlQuery"></param>
//        /// <param name="parameters"></param>
//        /// <returns>number of rows affected</returns>
//        public int DoSimpleCommand(string sqlQuery, SqlParameter[] parameters)
//        {
//            return Database.ExecuteSqlCommand(sqlQuery, parameters);
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<UserCompany>()
//                .Property(e => e.JobTitle)
//                .IsUnicode(false);

//            modelBuilder.Entity<CoursePreReqDetail>()
//                .Property(e => e.Course_Name)
//                .IsUnicode(false);

//            modelBuilder.Entity<CoursePreReqDetail>()
//                .Property(e => e.Course_PreReq_Name)
//                .IsUnicode(false);

//            modelBuilder.Entity<Form>()
//            .HasMany(e => e.FormVersions)
//            .WithRequired(e => e.Form)
//            .WillCascadeOnDelete(false);

//            modelBuilder.Entity<FormAttachment>()
//                .Property(e => e.Title)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormAttachment>()
//                .Property(e => e.Location)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormCompleted>()
//                .Property(e => e.FormData)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormField>()
//                .Property(e => e.Title)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormField>()
//                .HasMany(e => e.FormAttachments)
//                .WithRequired(e => e.FormField)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<FormSection>()
//                .Property(e => e.Title)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormVersion>()
//                .Property(e => e.Title)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormVersion>()
//                .Property(e => e.Description)
//                .IsUnicode(false);

//            modelBuilder.Entity<FormVersion>()
//                .HasMany(e => e.FormFields)
//                .WithRequired(e => e.FormVersion)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<FormVersion>()
//                .HasMany(e => e.FormTasks)
//                .WithRequired(e => e.FormVersion)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<AssessmentAnswer>()
//             .Property(e => e.Response)
//             .IsUnicode(false);

//            modelBuilder.Entity<AssessmentAnswer>()
//                .HasMany(e => e.AssessmentExceptions)
//                .WithRequired(e => e.AssessmentAnswer)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<AssessmentAnswer>()
//                .HasMany(e => e.AssessmentImages)
//                .WithRequired(e => e.AssessmentAnswer)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<AssessmentCompleted>()
//                .Property(e => e.PDF)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentCompleted>()
//                .HasMany(e => e.AssessmentAnswers)
//                .WithRequired(e => e.AssessmentCompleted)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<AssessmentCompleted>()
//                .HasMany(e => e.AssessmentMetrics)
//                .WithRequired(e => e.AssessmentCompleted)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<AssessmentException>()
//                .Property(e => e.ExceptionText)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentException>()
//                .Property(e => e.ResolutionText)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentImage>()
//                .Property(e => e.ImagePath)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentMetric>()
//                .Property(e => e.IP)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentMetric>()
//                .Property(e => e.Location)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentMetric>()
//                .Property(e => e.OS)
//                .IsUnicode(false);

//            modelBuilder.Entity<AssessmentMetric>()
//                .Property(e => e.Device)
//                .IsUnicode(false);

//            modelBuilder.Entity<JobMatrix>()
//               .Property(e => e.Title)
//               .IsUnicode(false);

//            modelBuilder.Entity<JobMatrix>()
//                .Property(e => e.Description)
//                .IsUnicode(false);
//        }

//        public System.Data.Entity.DbSet<OnBoardLMS.WebAPI.Models.AssessmentExceptionType> AssessmentExceptionTypes { get; set; }

//        public System.Data.Entity.DbSet<OnBoardLMS.WebAPI.Models.AssessmentResolutionType> AssessmentResolutionTypes { get; set; }

//        public Database Database { get; set; }
//    }
//}
