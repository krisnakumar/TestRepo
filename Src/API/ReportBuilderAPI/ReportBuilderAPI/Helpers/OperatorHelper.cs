using Amazon.Lambda.Core;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;

namespace ReportBuilderAPI.Helpers
{
    /// <summary>
    /// Class that helps to handle the different operator
    /// </summary>
    public static class OperatorHelper
    {
        /// <summary>
        /// List of fields that need to skip the value 
        /// </summary>
        private static readonly List<string> fields = new List<string>
        {
            Constants.ASSIGNED,
            Constants.COMPLETED,
            Constants.WORKBOOK_IN_DUE,
            Constants.PAST_DUE,
            Constants.SUPERVISOR_USER,
            Constants.NOT_SUPERVISORID,
            Constants.SUPERVISOR_ID,
            Constants.IN_DUE,
            Constants.IN_COMPLETE,
            Constants.NOT_COMPLETED_COMPANY_USERS,
            Constants.COMPLETED_COMPANY_USERS,
            Constants.ME,
            Constants.ME_AND_ALL_SUBORDINATES,
            Constants.ME_AND_DIRECT_SUBORDINATES,
            Constants.ALL_SUBORDINATES,
            Constants.DIRECT_SUBORDINATES,
            Constants.NOT_ME,
            Constants.NOT_ME_AND_ALL_SUBORDINATES,
            Constants.NOT_ME_AND_DIRECT_SUBORDINATES,
            Constants.NOT_ALL_SUBORDINATES,
            Constants.NOT_DIRECT_SUBORDINATES,
            Constants.ROLES,
            Constants.COMPANIES,
            Constants.COMPANY_USER_ID
        };

        /// <summary>
        /// Creates the query based on the operator Name
        /// </summary>
        /// <param name="operatorName"></param>
        /// <param name="value"></param>
        /// <param name="field"></param>
        /// <returns>string</returns>
        public static string CheckOperator(string operatorName, string value, string field)
        {
            string queryString = string.Empty;
            try
            {
                //Handles the like and between operator 
                if (!fields.Contains(field.ToUpper()) && value.ToUpper() != Constants.YES && value.ToUpper() != Constants.NO)
                {
                    switch (operatorName.ToUpper())
                    {
                        case Constants.CONTAINS:
                            queryString = " LIKE '%" + value + "%'";
                            break;
                        case Constants.DOES_NOT_CONTAINS:
                            queryString = " NOT LIKE '%" + value + "%'";
                            break;
                        case Constants.START_WITH:
                            queryString = " LIKE '" + value + "%'";
                            break;
                        case Constants.END_WITH:
                            queryString = " LIKE '%" + value + "'";
                            break;
                        case Constants.BETWEEN:
                            queryString = ProcessDateParameter(operatorName, value);
                            break;
                        default:
                            queryString = operatorName + ("'" + value + "'");
                            break;
                    }
                }
                return queryString;
            }
            catch (Exception operatorException)
            {
                LambdaLogger.Log(operatorException.ToString());
                return queryString;
            }
        }

        /// <summary>
        /// Separate the values if operator contains between
        /// </summary>        
        /// <param name="inputOperator"></param>
        /// <param name="value"></param>
        /// <returns>dateValue</returns>
        private static string ProcessDateParameter(string inputOperator, string value)
        {
            string dateValue = string.Empty;
            try
            {
                if (inputOperator.ToUpper() == Constants.BETWEEN)
                {
                    string[] dateList = value.ToUpper().Split("AND");
                    dateValue += " BETWEEN '" + dateList[0] + "'";
                    dateValue += " AND '" + dateList[1] + "'";
                }
                return dateValue;
            }
            catch (Exception processDateParameterException)
            {
                LambdaLogger.Log(processDateParameterException.ToString());
                return dateValue;
            }
        }
    }
}
