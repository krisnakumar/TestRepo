using Amazon.Lambda.Core;
using OnBoardLMS.WebAPI.Models;
using System;
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
        public UserCompany ValidateUser(int userId, int companyId)
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
    }
}
