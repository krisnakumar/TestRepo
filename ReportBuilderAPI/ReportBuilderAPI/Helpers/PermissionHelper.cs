using System;


namespace ReportBuilderAPI.Helpers
{

    [Serializable, Flags]
    public enum UserPerms : long //User Permissions
    {
        Add = 1, Edit = 2, Delete = 4, Disable = 8, Login = 16, ChangeID = 32, ViewOthers = 64, ViewProctorList = 128,
        EditOthers = 256,

        //Perms and Roles (not included in UserProfile.aspx -> Permissions Manager)
        ViewOwnPerms = 512, EditOwnPerms = 1024, ViewOwnRoles = 2048, EditOwnRoles = 4096, //Own
        ViewOthersPerms = 8192, EditOthersPerms = 16384, ViewOthersRoles = 32768, EditOthersRoles = 65536, //Others

        //add existing: special perm. not any old joe can have this 
        AddExisting = 131072,

        //Request permission upgrade. AKA: NGPA Become stakeholder. 
        PermissionRequestSent = 262144,

        //ForcePassword change: Allows a user to force a password change. 
        ForcePasswordChange = 524288,

        //override CompanySeries.Options to show all downloads instead of only for passed courses
        ShowAllAttachments = 1048576,

        //allow the user to override the workflow wait period and assign the exam
        UnlockExamFromWorkflow = 1 << 21, //2097152
        //merge user
        MergeUser = 1 << 22, //4194304
        ViewOthersForReport = 1 << 23, //8388608
        RemoveTaskFromProfile = 1 << 24, //16777216
        EditTaskProfile = 1 << 25, //33554432

        ViewExternalUsers = 1 << 26, //67108864
        ViewAdminUsers = 1 << 27, //134217728
        ImpersonateUsers = 1 << 28, //268435456
        EditEvents = 1 << 29,    //536870912
        EditTaskProfileRoleMapping = 1 << 30 //1073741824
    }
    [Serializable, Flags]
    public enum SettingsPerms : long //Settings Permissions
    {
        ChangePassword = 1, ChangeLandingPage = 2, ChangeDemographics = 4, CanApprovePermissionRequest = 8,
        EditForm = 16, ViewForm = 32, PublishForm = 64, CreateAssessment = 128, EditExceptions = 256, ViewExceptions = 512,
        ManageJobs = 1024, UserTransferRequests = 2048, EditPII = 4096, ManageApprovedTaskListRequests = 8192, ManageCertificateUploads = 16384,
        RequestApprovedTasks = 32768, UseNewAssignments = 65536, ManageWorkbooks = 131072, ScheduleReports = 262144, AggregatedCoachingReports = 524288,
        ManageRestrictedUsers = 1048576,
    }

    [Serializable, Flags]
    public enum CoursePerms : long //Course Permissions
    {
        Add = 1, Edit = 2, Delete = 4, Disable = 8, TakeCourse = 16, RegisterForCourse = 32, AssignToUser = 64,
        EditAssignment = 128, RemoveAssignment = 256, Purchase = 512, Proctor = 1024, ViewAssignment = 2048,
        ViewList = 4096, ReleaseOrder = 8192,

        //skill packets
        GetSkillPackets = 16384,

        //purchases
        ManagePurchases = 32768, //used in later version to view/manage purchases. This perm used in conjunction with CoursePerms.Purchase

        CreateRevision = 65536, //task revisions

        //manage tasks
        CreateTask = 131072, CreateTaskSkillAssociation = 262144,
        RemoveTaskSkillAssociation = 524288, RemoveTask = 1048576, EvaluateSkill = 2097152,

        //allowed to purchase by invoicing. Prevent regular users with the Purchase permission to be able to invoice. Only approved
        //personnel can make purchases that will be invoiced.  
        PurchaseWithInvoice = 4194304,

        //can view revisions
        ViewRevisions = 8388608,

        //can view test results
        ViewExamResults = 1 << 24, //16777216

        //can edit test results (requires user to have ViewResults and CompanySeries.Options to have EditResults
        EditExamResults = 1 << 25, //33554432

        //user can edit the assignment start date
        EditStartDate = 1 << 26 //67108864
    }
    [Serializable, Flags]
    public enum AnnouncementPerms : long //Announcement Permissions
    {
        Add = 1, Edit = 2, Delete = 4, Disable = 8, View = 16
    }
    [Serializable, Flags]
    public enum TranscriptPerms : long //Transcript Permissions
    {
        //Rename Delete to Archive as it is being repurposed
        Add = 1, Edit = 2, Archive = 4, ViewOwn = 8, ViewOthers = 16, Print = 32, Export = 64, Email = 128,

        //each report will be visible depending on the perm level of the user
        //Changed CanQualifyAndSuspend to just CanQualify.  Also replaced ViewIntermediate (wasn't being used anywhere) with "new" perm CanSuspend.
        ViewQualifiedModules = 256, ViewBasic = 512, CanSuspend = 1024, ViewDetailed = 2048,
        ViewExpiring = 4096, ViewExpired = 8192, ViewArchives = 16384,
        ViewGap = 32768,
        //by task
        ViewQualifiedTasks = 65536, ViewDetailedTask = 131072,
        ViewDetailedTaskGrouped = 262144, ViewQualifiedTaskGrouped = 524288,
        ViewBasicTasks = 1048576, ViewExpiringTasks = 2097152,
        ViewExpiredTasks = 4194304,

        //AddSkill: Can upload skills to the LMS (ManualSkillUpload.aspx)
        AddSkill = 8388608,

        //ManualSkillUpload page, Gap report (submit skill) feature. Allows admin to bypass the skill submit period. 
        IgnoreSkillSubmitPeriod = 1 << 24, //16777216
                                           //Can Suspend and Qualify users from detailed module
                                           //Changed CanQualifyAndSuspend to just CanQualify.  Also replaced ViewIntermediate (wasn't being used anywhere) with "new" perm CanSuspend.
        CanQualify = 1 << 25, //33554432
        ViewEvaluationException = 1 << 26, //67108864
        ViewEvaluation = 1 << 27, //134217728
        ViewWalletCardScan = 1 << 28, //268435456
        ViewDisqualificationReport = 1 << 29, //536870912
        ViewSuspensionReport = 1 << 30 //1073741824
    }
    [Serializable, Flags]
    public enum CompanyPerms : long //Company Permissions
    {
        Add = 1, Edit = 2, Delete = 4, Disable = 8, AddSite = 16, EditSite = 32, DeleteSite = 64,
        DisableSite = 128, AddDept = 256, EditDept = 512, EditSupervisor = 1024, DisableDept = 2048,
        ViewCompanies = 4096, ViewCredits = 8192,

        EditCompanyClient = 16384, //manage client companies

        //orders
        ViewOrders = 32768, //orders.aspx
        RemoveOrder = 65536, //remove order from orders.aspx

        LimitedCompanySeries = 131072, //Can only view series specified in UserCompanySeries
        PurchaseVoucher = 262144,  //Allows a user to purchase vouchers within a company

        //Project management. 
        CreateProject = 1 << 19, //524288
        EditProject = 1 << 20, //1048576
        ViewProject = 1 << 21, //2097152
        GenerateProctorPasswords = 1 << 22, //4194304
        SearchByUsernameMobile = 1 << 23, //8388608
        EditCostCenters = 1 << 24, //16777216
        EditWorkLocations = 1 << 25, //33554432
        ViewCostCenters = 1 << 26, //671088614
        ViewWorkLocations = 1 << 27, //134217728
        EditCompanyContacts = 1 << 28, //268435456   Used for Company Contacts page
        ViewCompanyContacts = 1 << 29, //536870912   Used for Company Contacts page
        EditEvaluatorCredentials = 1 << 30 //1073741824 Proctor password page. Can add/edit/remove evaluator credentials
    }
    [Serializable, Flags]
    public enum ForumPerms : long //Forum Permissions
    {
        Add = 1, Edit = 2, Delete = 4, Disable = 8, View = 16,
        AddTopic = 32, EditTopic = 64, DeleteTopic = 128, DisableTopic = 256, ViewTopic = 512,
        AddMessage = 1024, EditMessage = 2048, DeleteMessage = 4096, DisableMessage = 8192, ViewMessage = 16384
    }
    [Serializable, Flags]
    public enum ComPerms : long //Communication Permissions
    {
        SendEmail = 1, ContactSupervisor = 2, SubmitSupportTicket = 4, ContactOtherUsers = 8
    }
    [Serializable, Flags]
    public enum ReportPerms : long //Report Permissions
    {
        View = 1, Print = 2, Export = 4, Disable = 8, CrossCompanyReports = 16,

        //each report will be visible depending on the perm level of the user
        FailedModule = 32, MissingSkills = 64,

        //search options
        SearchOptionStudent = 128, SearchOptionCourse = 256, SearchOptionDate = 512,
        SearchOptionOrderNumber = 1024, SearchOptionProctor = 16384, SearchOptionRole = 32768,

        //other report types
        CourseUsageReports = 2048, ExceptionReports = 4096,

        //create reports (custom reports)
        Add = 8192,

        //new export option: excel separate
        ExportExcel = 65536,
        CollapseSkills = 131072,

        //usage reports: item analysis
        ViewItemAnalysis = 262144,

        //no limit on number of records to export
        NoExportLimit = 524288,

        //more search options
        SearchOptionDepartment = 1048576,
        SearchOptionSeries = 2097152,
        SearchOptionTask = 4194304,
        EvalExceptionReports = 8388608,
        SearchOptionSupervisor = 1 << 24, //16777216
        ViewAuditHistory = 1 << 25, //33554432
        SearchOptionCostCenter = 1 << 26, //67108864
        SearchOptionWorkLocation = 1 << 27, //134217728
        ShowDashBoard = 1 << 28, //268435456
        //ISNUpload report access permission
        ISNUpload = 1 << 29, //536870912
        ViewNCMSStatusReport = 1 << 30, //1073741824,
        ViewAssessmentDashboard = 2147483648,
        ViewAssessmentReport = 4294967296,
        ViewAssessmentExceptionReport = 8589934592,
        ViewDataAudit = 17179869184, //Data Audits
        ViewEvaluatorApprovedTaskReport = 34359738368,
        ViewCoachingReport = 68719476736,
        ViewJobReport = 137438953472L,
        ShowDashBoardTopLevel = 274877906944L,
        ViewWorkbookReport = 549755813888L,
        ViewContractorOQDashboard = 1099511627776L,
        ViewContractorTrainingDashboard = 2199023255552L,
        ViewQueryBuilder = 4398046511104L,
        ViewWorkbooksDashboard = 8796093022208L
    }
    [Serializable, Flags]
    public enum SystemPerms : long //System Permissions
    {
        Maintenance = 1, ErrorLogs = 2, Transaction = 4, SystemHealth = 8, ReceiveNotifications = 16,

        /// <summary>
        /// Manage business functions in OnBoard (OnBoard Business page) such as viewing expiring contracts, 
        /// renewing contracts, etc
        /// </summary>
        ManageBusiness = 32,

        //view only users who you supervise
        ViewSubordinates = 64,
        ChangeAPI = 128,
        RetrievePassword = 256,
        SetRemoteId = 512,
        /// <summary>
        /// Allows this admin to transfer assignments
        /// </summary>
        TransferAssignment = 1024,
        /// <summary>
        /// allows this admin to access SystemSearch
        /// </summary>
        SystemSearch = 2048,
        /// <summary>
        /// Allows this admin to Generate a QR Code
        /// </summary>
        GenerateQRCode = 4096,
        /// <summary>
        /// Allows this admin to Reset Demographics (address and email)
        /// </summary>
        ResetDemographics = 8192
    }

}
