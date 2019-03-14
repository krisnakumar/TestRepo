using Amazon.Lambda.Core;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportBuilderAPI.Helpers
{
    /// <summary>
    /// class that helps to the get the sql parameters
    /// </summary>
    public static class ParameterHelper
    {
        /// <summary>
        /// Get the list of sql parameters
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Getparameters(QueryBuilderRequest queryRequest)
        {
            //Parameters which used to create query
            string supervisorId = string.Empty, workbookId = string.Empty, taskId = string.Empty, dueDays = string.Empty, role = string.Empty;
            Dictionary<string, string> parameterList = new Dictionary<string, string> { };
            try
            {
                //Get the list of Id details
                supervisorId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault();
                workbookId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.WORKBOOK_ID).Select(x => x.Value).FirstOrDefault();
                taskId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.TASK_ID).Select(x => x.Value).FirstOrDefault();
                dueDays = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == (Constants.WORKBOOK_IN_DUE) || x.Name.ToUpper() == (Constants.PAST_DUE)).Select(x => x.Value).FirstOrDefault());
                role = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ROLE_ID).Select(x => x.Value).FirstOrDefault());

                //Set default due days if its null
                if (string.IsNullOrEmpty(dueDays))
                {
                    dueDays = "30";
                }

                //Get the parameter dictionary
                parameterList = new Dictionary<string, string>() { { "userId", Convert.ToString(supervisorId) }, { "companyId", Convert.ToString(queryRequest.CompanyId) }, { "workbookId", Convert.ToString(workbookId) }, { "taskId", Convert.ToString(taskId) }, { "duedays", Convert.ToString(dueDays) }, { "role", Convert.ToString(role) } };
                return parameterList;
            }
            catch (Exception getParameterException)
            {
                LambdaLogger.Log(getParameterException.ToString());
                return parameterList;
            }
        }
    }
}
