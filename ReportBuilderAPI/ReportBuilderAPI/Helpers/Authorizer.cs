using Amazon.Lambda.Core;
using DataInterface.Database;
using OnBoardLMS.Web;
using OnBoardLMS.WebAPI.Models;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportBuilderAPI.Helpers
{
    /// <summary>
    /// Class that helps to validate the user against company
    /// </summary>
    public class Authorizer : DatabaseWrapper
    {
        /// <summary>
        /// Validate the user against company
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        public bool ValidateUser(int userId, int companyId, string appType = "", string studentDetails = "")
        {
            try
            {
                using (DBEntity context = new DBEntity())
                {
                    //check whether the user has access to the company  
                    UserCompany userCompany = (from uc in context.UserCompany
                                               where uc.CompanyId == companyId
                                               && uc.IsEnabled && uc.Status == 1
                                               && uc.UserId == userId
                                               select uc).FirstOrDefault();
                    if (userCompany != null)
                    {
                        //Validate the user report permissions
                        PermissionManager permissionManager = new PermissionManager(Convert.ToInt64(userCompany.ReportsPerms));
                        if (CheckReportPermissions(permissionManager, appType))
                        {
                            if (appType == Constants.QUERY_BUILDER && !string.IsNullOrEmpty(studentDetails))
                            {
                                if (IsMyStudent(studentDetails, userCompany, userId, companyId))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (appType == Constants.TRAINING_DASHBOARD || appType == Constants.OQ_DASHBOARD)
                        {
                            bool clientCompany = (from uc in context.UserCompany
                                                  join cc in context.CompanyClient on uc.CompanyId equals cc.OwnerCompany
                                                  where uc.IsDefault && uc.IsEnabled && uc.Status == 1 && cc.IsEnabled && uc.UserId == userId && cc.ClientCompany == companyId
                                                  select uc.UserId).ToList().Count > 0;
                            if (clientCompany)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception validateUserException)
            {
                LambdaLogger.Log(validateUserException.ToString());
                return false;
            }
        }

        /// <summary>
        /// Check report permissions
        /// </summary>
        /// <param name="permissionManager"></param>
        /// <returns></returns>
        public bool CheckReportPermissions(PermissionManager permissionManager, string appType)
        {
            bool hasPermission = false;
            try
            {
                switch (appType)
                {
                    case Constants.OQ_DASHBOARD:
                        hasPermission = permissionManager.Contains(ReportPerms.ViewContractorOQDashboard);
                        break;
                    case Constants.TRAINING_DASHBOARD:
                        hasPermission = permissionManager.Contains(ReportPerms.ViewContractorTrainingDashboard);
                        break;
                    case Constants.WORKBOOK_DASHBOARD:
                        hasPermission = permissionManager.Contains(ReportPerms.ViewWorkbooksDashboard);
                        break;
                    case Constants.QUERY_BUILDER:
                        hasPermission = permissionManager.Contains(ReportPerms.ViewQueryBuilder);
                        break;
                    default:
                        hasPermission = false;
                        break;
                }
                return hasPermission;
            }
            catch (Exception reportException)
            {
                LambdaLogger.Log(reportException.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentDetails"></param>
        /// <param name="userCompany"></param>
        /// <returns></returns>
        public bool IsMyStudent(string studentDetails, UserCompany userCompany, int userId, int companyId, bool EnabledUsersOnly = true, bool DirectSupervisionUsersOnly = false, string altStudentQuery = "", bool RequiresCompanyManagePerm = true, bool RequireIsVisible = true)
        {
            int userDetails = 0, studentCount = 0;            
            try
            {
                //validate
                if (!Validator.Valid(studentDetails, typeof(DatabaseWrapper)) && altStudentQuery == "")
                {
                    throw new ArgumentException("student list");
                }

                if (!Validator.Valid(altStudentQuery, typeof(string)) && studentDetails == "")
                {
                    throw new ArgumentException("alt Student Query");
                }
                if (studentDetails == "")
                {
                    studentCount = Validator.GetInt(ExecuteScalar(string.Format("select count(*) from ( {0} ) T1;", altStudentQuery)));
                }
                else
                {
                    studentCount = studentDetails.Split(',').Length;
                }


                if (userDetails == Validator.GetInt(studentDetails))
                {
                    return true;
                }
                else
                {
                    List<int> students = studentDetails.Split(',').Select(int.Parse).ToList();
                    using (DBEntity dBEntity = new DBEntity())
                    {

                        IQueryable<int> isMyStudent = (from uc in dBEntity.UserCompany
                                                       join ucm in dBEntity.UserCompany on uc.CompanyId equals ucm.CompanyId
                                                       where ucm.UserId == userId && ucm.CompanyPerms == (int)CompanyPerms.Edit
                                                       && ucm.IsEnabled && ucm.Status == 1 && uc.CompanyId == companyId && uc.IsVisible && uc.IsEnabled
                                                       && students.Contains(uc.UserId)
                                                       select uc.UserId);
                        if ((userCompany.ReportsPerms & (int)ReportPerms.CrossCompanyReports) == (int)ReportPerms.CrossCompanyReports)
                        {

                            IQueryable<int> isMyCrossStudent = (from uc in dBEntity.UserCompany
                                                                join cc in dBEntity.CompanyClient on uc.CompanyId equals cc.ClientCompany
                                                                join ucm in dBEntity.UserCompany on cc.OwnerCompany equals ucm.CompanyId
                                                                where ucm.UserId == userId && ucm.CompanyPerms == (int)CompanyPerms.Edit
                                                                && ucm.IsEnabled && ucm.Status == 1 && ucm.CompanyId == companyId
                                                                && students.Contains(uc.UserId)
                                                                && uc.IsVisible && uc.IsEnabled && uc.IsVisible && uc.Status == 1
                                                                select uc.UserId);
                            isMyStudent = isMyStudent.Union(isMyCrossStudent);
                        }

                        bool result = isMyStudent.Count() > 0;
                        return result;
                    }
                }
            }
            catch (Exception studentException)
            {
                LambdaLogger.Log(studentException.ToString());
                return false;
            }
        }
    }
}
