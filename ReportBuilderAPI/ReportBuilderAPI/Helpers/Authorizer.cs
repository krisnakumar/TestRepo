﻿using Amazon.Lambda.Core;
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
    public class Authorizer
    {
        /// <summary>
        /// Validate the user against company
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        public UserCompany ValidateUser(int userId, int companyId, string appType = "", string studentDetails = "")
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
                        if (CheckReportPermissions(permissionManager))
                        {
                            if (appType == Constants.QUERY_BUILDER && !string.IsNullOrEmpty(studentDetails))
                            {
                                if (IsMyStudent(studentDetails, userCompany, userId, companyId))
                                {
                                    return userCompany;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            return userCompany;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return userCompany;
                }
            }
            catch (Exception validateUserException)
            {
                LambdaLogger.Log(validateUserException.ToString());
                return null;
            }
        }

        /// <summary>
        /// Check report permissions
        /// </summary>
        /// <param name="permissionManager"></param>
        /// <returns></returns>
        public bool CheckReportPermissions(PermissionManager permissionManager)
        {
            try
            {
                if (permissionManager.Contains(ReportPerms.ViewContractorOQDashboard)
                            || permissionManager.Contains(ReportPerms.ViewContractorTrainingDashboard) || permissionManager.Contains(ReportPerms.ViewWorkbooksDashboard) || permissionManager.Contains(ReportPerms.ViewQueryBuilder))
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
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
                    studentCount = Validator.GetInt(databaseWrapper.ExecuteScalar(string.Format("select count(*) from ( {0} ) T1;", altStudentQuery)));
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
