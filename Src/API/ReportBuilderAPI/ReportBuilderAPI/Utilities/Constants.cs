﻿

namespace ReportBuilderAPI.Utilities
{
    /// <summary>
    ///     Class that handles the constants value
    /// </summary>
    public static class Constants
    {

        public const string CompletedWorkBooks = "COMPLETEDWORKBOOKS";
        public const string WorkBookInDue = "WORKBOOKINDUE";
        public const string PastDueWorkBook = "PASTDUEWORKBOOK";

        public const string PARAM = "PARAM";


        public const string QUERY_BUILDER = "QUERY_BUILDER";

        public const string CONTAINS = "CONTAINS";
        public const string DOES_NOT_CONTAINS = "DOES NOT CONTAIN";

        public const string START_WITH = "STARTS WITH";
        public const string END_WITH = "ENDS WITH";

        public const string ID = "ID";
        public const string ROLEID = "ROLEID";
        public const string USERNAME = "USERNAME";
        public const string ALTERNATE_USERNAME = "ALTERNATE_USERNAME";
        public const string USERNAME2 = "USERNAME2";
        public const string DATECREATED = "DATECREATED";
        public const string FIRSTNAME = "FIRSTNAME";
        public const string MIDDLENAME = "MIDDLENAME";
        public const string LASTNAME = "LASTNAME";
        public const string EMAIL = "EMAIL";
        public const string ADDRESS = "ADDRESS";
        public const string CITY = "CITY";
        public const string STATE = "STATE";
        public const string PHONE = "PHONE";
        public const string ZIP = "ZIP";
        public const string COUNTRY = "COUNTRY";
        public const string SUPERVISOR_ID = "SUPERVISOR_ID";

        public const string TRAINING_ROLE = "TRAINING_ROLE";

        public const string NOT_SUPERVISORID = "NOT_SUPERVISORID ";
        public const string SUPERVISOR_USER = "SUPERVISOR_USER";
        public const string SUPERVISOR_ID_HANDLER = "SUPERVISOR_ID_HANDLER";

        public const string SUPERVISOR = "SUPERVISOR";
        public const string COMPANY_NAME = "COMPANY_NAME";

        public const string SUPERVISOR_SUB = "SUPERVISOR_SUB";
        public const string ROLE = "ROLE";

        public const string ROLES = "ROLES";
        public const string COMPANIES = "COMPANIES";


        public const string CAN_CERTIFY = "CAN_CERTIFY";

        public const string ISDIRECTREPORT = "ISDIRECTREPORT";
        public const string ISENABLED = "ISENABLED";
        public const string PHOTO = "PHOTO";
        public const string ISNETWORKID = "ISNETWORKID";
        public const string COMPANYNAME = "COMPANYNAME";
        public const string COMPANYID = "COMPANYID";
        public const string COMPANYCREATED = "COMPANYCREATED";
        public const string LICENCETYPE = "LICENCETYPE";
        public const string CONTRACTDATE = "CONTRACTDATE";
        public const string DATEPAID = "DATEPAID";

        public const string EMPLOYEE_NAME = "EMPLOYEE_NAME";
        public const string SUPERVISOR_NAME = "SUPERVISOR_NAME";
        public const string CONTRACTOR_NAME = "CONTRACTOR_NAME";
        public const string ASSIGNED_WORKBOOK = "ASSIGNED_WORKBOOK";
        public const string TASK_PROGRESS = "TASK_PROGRESS";
        public const string REP_PROGRESS = "REP_PROGRESS";
        public const string WORKBOOK_NAME = "WORKBOOK_NAME";
        public const string WORKBOOK_DUE = "WORKBOOK_DUE";
        public const string PAST_DUE_WORKBOOK = "PAST_DUE_WORKBOOK";
        public const string COMPLETED_WORKBOOK = "COMPLETED_WORKBOOK";
        public const string INCOMPLETE_WORKBOOK = "INCOMPLETE_WORKBOOK";
        public const string INCOMPLETE_TASK = "INCOMPLETE_TASK";
        public const string TOTAL_TASK = "TOTAL_TASK";
        public const string TOTAL_WORKBOOK = "TOTAL_WORKBOOK";

        public const string TOTAL_EMPLOYEES = "TOTAL_EMPLOYEES";
        public const string USER_CREATED_DATE = "USER_CREATED_DATE";

        public const string USER_PERMS = "USER_PERMS";
        public const string SETTINGS_PERMS = "SETTINGS_PERMS";
        public const string COURSE_PERMS = "COURSE_PERMS";
        public const string TRANSCRIPT_PERMS = "TRANSCRIPT_PERMS";
        public const string COMPANY_PERMS = "COMPANY_PERMS";
        public const string FORUM_PERMS = "FORUM_PERMS";
        public const string COM_PERMS = "COM_PERMS";
        public const string REPORTS_PERMS = "REPORTS_PERMS";
        public const string ANNOUNCEMENT_PERMS = "ANNOUNCEMENT_PERMS";
        public const string SYSTEM_PERMS = "SYSTEM_PERMS";

        public const string USERID = "USER_ID";

        public const string COMPANY_USER_ID = "COMPANY_USER_ID";

        public const string CURRENT_USER = "CURRENT_USER";
        public const string STUDENT_DETAILS = "STUDENT_DETAILS";
        //Fields and Columns for the Employee Entity
        public const string DESCRIPTION = "DESCRIPTION";
        public const string WORKBOOK_CREATED = "WORKBOOK_CREATED";
        public const string WORKBOOK_ISENABLED = "WORKBOOK_ISENABLED";
        public const string WORKBOOK_CREATED_BY = "WORKBOOK_CREATED_BY";
        public const string DAYS_TO_COMPLETE = "DAYS_TO_COMPLETE";
        public const string WORKBOOK_ID = "WORKBOOK_ID";

        public const string ENTITY_COUNT = "ENTITY_COUNT";
        public const string USER_COUNT = "USER_COUNT";
        public const string WORKBOOK_ASSIGNED_DATE = "WORKBOOK_ASSIGNED_DATE";
        public const string NUMBER_COMPLETED = "NUMBER_COMPLETED";
        public const string BETWEEN = "BETWEEN";

        public const string LAST_ATTEMPT_DATE = "LAST_ATTEMPT_DATE";
        public const string FIRST_ATTEMPT_DATE = "FIRST_ATTEMPT_DATE";

        public const string REPETITIONS = "REPETITIONS";
        public const string LAST_SIGNOFF_BY = "LAST_SIGNOFF_BY";
        public const string COMPLETED_TASK = "COMPLETED_TASK";

        public const string DUE_DATE = "DUE_DATE";


        public const string ASSIGNED_TO = "ASSIGNED_TO";
        public const string DATE_ADDED = "DATE_ADDED";

        public const string ASSIGNED = "ASSIGNED";

        public const string WORKBOOK_DETAILS = "WORKBOOK_DETAILS";

        public const string NULL = "NULL";
        public const string COMPLETED = "COMPLETED";
        public const string LOCKOUT_COUNT = "LOCKOUT_COUNT";
        public const string NOT_COMPLETED = "NOT COMPLETED";

        public const string QUALIFIED = "QUALIFIED";
        public const string NOT_QUALIFIED = "NOT QUALIFIED";

        public const string WORKBOOK_IN_DUE = "WORKBOOK_IN_DUE";
        public const string PAST_DUE = "PAST_DUE";
        public const string IN_DUE = "IN_DUE";
        public const string DUE_DAYS = "DUE_DAYS";


        public const string QR_CODE = "QR_CODE";

        public const string DEPARTMENT = "DEPARTMENT";
        //Fields for tasks
        public const string TASK_NAME = "TASK_NAME";

        public const string LOCK_OUT_REASON = "LOCK_OUT_REASON";
        public const string LOCK_OUT_COUNT = "LOCK_OUT_COUNT";
        public const string COMPANY_LOCK_OUT_COUNT = "COMPANY_LOCK_OUT_COUNT";

        public const string DISQUALIFIED = "DISQUALIFIED";
        public const string SUSPENDED = "SUSPENDED";

        public const string TASK_CODE = "TASK_CODE";
        public const string TASK_ID = "TASK_ID";
        public const string TASK_CREATED = "TASK_CREATED";
        public const string ATTEMPT_DATE = "ATTEMPT_DATE";
        public const string EVALUATOR_NAME = "EVALUATOR_NAME";
        public const string DATE_TAKEN = "DATE_TAKEN";
        public const string DATE_EXPIRED = "DATE_EXPIRED";
        public const string STATUS = "STATUS";
        public const string REPORTING = "REPORTING";
        public const string CREATED_BY = "CREATED_BY";
        public const string DELETED_BY = "DELETED_BY";
        public const string REPETITIONS_COUNT = "REPETITIONS_COUNT";
        public const string LOCATION = "LOCATION";
        public const string IP = "IP";
        public const string DURATION = "DURATION";
        public const string SCORE = "SCORE";
        public const string COMPLETION_DATE = "COMPLETION_DATE";
        public const string PARENT_TASK_NAME = "PARENT_TASK_NAME";
        public const string CHILD_TASK_NAME = "CHILD_TASK_NAME";
        public const string NUMBER_OF_ATTEMPTS = "NUMBER_OF_ATTEMPTS";
        public const string EXPIRATION_DATE = "EXPIRATION_DATE";
        public const string COURSE_EXPIRATION_DATE = "COURSE_EXPIRATION_DATE";
        public const string COMMENTS = "COMMENTS";

        //Tasks for Qualification
        public const string ASSIGNED_QUALIFICATION = "ASSIGNED_QUALIFICATION";
        public const string COMPLETED_QUALIFICATION = "COMPLETED_QUALIFICATION";

        public const string SUSPENDED_QUALIFICATION = "SUSPENDED_QUALIFICATION";
        public const string DISQUALIFIED_QUALIFICATION = "DISQUALIFIED_QUALIFICATION";

        public const string IN_COMPLETE_QUALIFICATION = "IN_COMPLETE_QUALIFICATION";
        public const string IN_COMPLETE = "IN_COMPLETE";

        public const string PAST_DUE_QUALIFICATION = "PAST_DUE_QUALIFICATION";
        public const string IN_DUE_QUALIFICATION = "IN_DUE_QUALIFICATION";
        public const string ASSIGNED_DATE = "ASSIGNED_DATE";

        public const string COMPANY_ID = "COMPANY_ID";

        public const string ASSIGNED_COMPANY_QUALIFICATION = "ASSIGNED_COMPANY_QUALIFICATION";
        public const string COMPLETED_COMPANY_QUALIFICATION = "COMPLETED_COMPANY_QUALIFICATION";
        public const string IN_COMPLETE_COMPANY_QUALIFICATION = "IN_COMPLETE_COMPANY_QUALIFICATION";

        public const string PAST_DUE_COMPANY_QUALIFICATION = "PAST_DUE_COMPANY_QUALIFICATION";
        public const string IN_DUE_COMPANY_QUALIFICATION = "IN_DUE_COMPANY_QUALIFICATION";
        public const string TOTAL_COMPANY_EMPLOYEES = "TOTAL_COMPANY_EMPLOYEES";

        public const string COMPLETED_ROLE_QUALIFICATION = "COMPLETED_ROLE_QUALIFICATION";

        public const string COMPLETED_COMPANY_USERS = "COMPLETED_COMPANY_USERS";
        public const string NOT_COMPLETED_COMPANY_USERS = "NOT_COMPLETED_COMPANY_USERS";
        public const string TOTAL_COMPLETED_COMPANY_USERS = "TOTAL_COMPLETED_COMPANY_USERS";

        public const string IS_SHARED = "IS_SHARED";
        public const string FAILED = "FAILED";
        

        public const string ROLE_ID = "ROLE_ID";

        public const string ALL = "ALL";

        public const string CONTRACTOR_COMPANY = "CONTRACTOR_COMPANY";

        public const string ADMIN_ID = "ADMIN_ID";
        public const string NOT_COMPLETED_ROLE_QUALIFICATION = "NOT_COMPLETED_ROLE_QUALIFICATION";

        //Smart parameters for  username

        public const string ME = "ME";
        public const string ME_AND_DIRECT_SUBORDINATES = "ME_AND_DIRECT_SUBORDINATES";
        public const string DIRECT_SUBORDINATES = "DIRECT_SUBORDINATES";
        public const string ALL_SUBORDINATES = "ALL_SUBORDINATES";
        public const string ME_AND_ALL_SUBORDINATES = "ME_AND_ALL_SUBORDINATES";


        public const string NOT_ME = "NOT_ME";
        public const string NOT_ME_AND_DIRECT_SUBORDINATES = "NOT_ME_AND_DIRECT_SUBORDINATES";
        public const string NOT_DIRECT_SUBORDINATES = "NOT_DIRECT_SUBORDINATES";
        public const string NOT_ALL_SUBORDINATES = "NOT_ALL_SUBORDINATES";
        public const string NOT_ME_AND_ALL_SUBORDINATES = "NOT_ME_AND_ALL_SUBORDINATES";

        //List of entities
        public const string TASK = "TASK";
        public const string WORKBOOK = "WORKBOOK";
        public const string EMPLOYEE = "EMPLOYEE";


        public const string YES = "YES";
        public const string NO = "NO";

        public const string IS_NULL = " IS NULL";
        public const string IS_NOT_NULL = " IS NOT NULL";

        public const string DUE_DAYS_VALUE = "30";
        public const string TRAINING_DASHBOARD = "TRAINING_DASHBOARD";

        public const string WORKBOOK_DASHBOARD = "WORKBOOK_DASHBOARD"; 
        public const string OQ_DASHBOARD = "OQ_DASHBOARD";

        public const string APP_TYPE = "APPTYPE";
        public const string PAYLOAD = "PAYLOAD";

        public const string SQLException = "SQLException";

        public const string DEFAULT_MESSAGE = "DEFAULT_MESSAGE";

    }
}
