using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using OnBoardLMS.WebAPI.Models;
using System;

namespace ReportBuilderAPI.IRepository
{
    public interface IDBEntity : IDisposable, IObjectContextAdapter
    {
        DbSet<UserCompany> UserCompanies { get; set; }
        DbSet<CoursePreReqDetail> CoursePreReqDetails { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<EventCourse> EventCourses { get; set; }
        DbSet<EventRoster> EventRosters { get; set; }
        DbSet<EventOrder> EventOrders { get; set; }
        DbSet<UserCompanyDetail> UserCompanyDetails { get; set; }
        DbSet<EvaluatorApprovedTask> EvaluatorApprovedTasks { get; set; }
        DbSet<EvaluatorApprovedCourse> EvaluatorApprovedCourses { get; set; }
        DbSet<TaskAssociationDetail> TaskAssociationDetails { get; set; }
        DbSet<CourseDetail> CourseDetails { get; set; }
        DbSet<CourseSkill> CourseSkills { get; set; }
        DbSet<EvaluatorApprovedTaskByCourse> EvaluatorApprovedTaskByCourses { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<CourseAccess> CourseAccesss { get; set; }
        DbSet<CompanyCourse> CompanyCourses { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductCourse> ProductCourses { get; set; }
        DbSet<CourseAssignment> CourseAssignments { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<TaskRole> TaskRoles { get; set; }
        DbSet<TaskRoleAssociation> TaskRoleAssociations { get; set; }
        DbSet<UserCompanySeries> UserCompanySeries { get; set; }
        DbSet<Series> Series { get; set; }

        DbSet<DataAudit> DataAudits { get; set; }
        DbSet<DataAuditReport> DataAuditReports { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<CompanyClient> CompanyClients { get; set; }
        DbSet<TestSession> TestSessions { get; set; }
        DbSet<TestSessionAudit> TestSessionAudits { get; set; }
        DbSet<CompanyIntegration> CompanyIntegrations { get; set; }
        DbSet<SkillActivity> SkillActivities { get; set; }
        DbSet<SkillActivityDraft> SkillActivityDrafts { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<CompanySeries> CompanySeriess { get; set; }
        DbSet<SkillMethod> SkillMethods { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<CourseRole> CourseRoles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<JobMatrix> JobMatrices { get; set; }
        DbSet<JobMatrixTask> JobMatrixTasks { get; set; }
        DbSet<UserCompanyTransfer> UserCompanyTransfers { get; set; }
        DbSet<Acknowledgement> Acknowledgements { get; set; }
        DbSet<CompanyCodeCourseMapping> CompanyCodeCourseMappings { get; set; }
        DbSet<PIIField> PIIFields { get; set; }
        DbSet<PIICompanyField> PIICompanyFields { get; set; }
        DbSet<EvaluatorApproval> EvaluatorApprovals { get; set; }
        DbSet<Reference> References { get; set; }
        DbSet<ApprovalReference> ApprovalReferences { get; set; }
        DbSet<Approval> Approvals { get; set; }
        DbSet<ApprovalDenialReason> ApprovalDenialReasons { get; set; }
        DbSet<FailedSkillActivity> FailedSkillActivities { get; set; }
        DbSet<Workbook> Workbooks { get; set; }
        DbSet<WorkbookContent> WorkbookContents { get; set; }
        DbSet<UserWorkbook> UserWorkbooks { get; set; }
        DbSet<WorkbookProgress> WorkbookProgresses { get; set; }
        DbSet<WorkbookAudit> WorkbookAudits { get; set; }
        DbSet<CourseAttachment> CourseAttachments { get; set; }
        DbSet<ScheduledReport> ScheduledReports { get; set; }
        DbSet<ScheduledReportDetail> ScheduledReportDetails { get; set; }
        DbSet<ScheduledReportAudit> ScheduledReportAudits { get; set; }
        DbSet<RestrictedUser> RestrictedUsers { get; set; }

        void Dispose();

        void SetModified(object entity);

        void SetAdded(object entity);

        int SaveChanges();

        int DoSimpleCommand(string sqlQuery, SqlParameter[] parameters);



        Database Database { get; set; }

        List<UserDefaultDetail> GetUserDefaultDetails(int id);
        List<UserTranscriptDetail> GetUserTranscriptDetails(int id);
        List<EventDetail> GetEventDetails(int eventId);
        List<SkillActivityDraftReport> GetCompanyDrafts(int companyId);
        List<ShareLink> GetShareLinks(int eventId, int linkType);
        List<TaskRoleMappingValidation> GetTaskRoleMappingValidations(int roleId, int taskRoleId, int mainTaskId, int mappedTaskId);
        List<TaskRoleAssociationDetail> GetTaskRoleAssociationDetails(int taskRoleId);
        List<AssignmentDetail> GetAssignmentsForUser(int userId, int companyId, bool returnExpired = false);
        List<ItemAnalysisSummaryDetail> GetItemAnalysisSummaryDetails(int seriesId);
        List<EvaluatorApprovedTasksReport> GetEvaluatorApprovedTaskListDetails(int userId, int companyId, string rptFilters);
        List<JobReport> GetJobReport(int companyId, string rptFilters);
        List<UserCompanyTransferDetail> GetEscalatedUserCompanyTransfers(int companyId);
        List<UserDefaultDetail> SearchForUser(int companyId, string term, int top = 100);
        List<TaskRequirement> GetTaskRequirements(int companyId, bool currentOnly, int userId = -1);
        List<Skill> GetEvaluatorAvailableSkills(int evaluatorId, int seriesId);
        List<CourseSkillDetail> GetEvaluatorAvailableCourses(int evaluatorId, int seriesId);
        List<WorkbookReport> GetWorkbookReport(int companyId, string rptFilters);
        List<Skill> GetEvaluatorSkillsBySeries(int evaluatorId, int seriesId);
        List<CompanyUserDetails> GetCompanyUserDetails(int userId, int companyId, string rptFilters);
        List<RestrictedUsersReport> GetRestrictedUsersByCompany(int companyId);
    } 
}