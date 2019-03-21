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
                    UserCompany userCompany = (from uc in context.UserCompany
                                               where uc.CompanyId == companyId
                                               && uc.IsEnabled && uc.Status == 1
                                               select uc).FirstOrDefault();
                    if (userCompany.CompanyPerms == 1 && userCompany.UserPerms == 1)
                    {
                        return userCompany;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception validateUserException)
            {
                LambdaLogger.Log(validateUserException.ToString());
                return null;
            }
        }
    }
}
