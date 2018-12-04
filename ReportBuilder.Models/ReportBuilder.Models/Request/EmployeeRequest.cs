using ReportBuilder.Models.Models;
using System.Collections.Generic;

// <copyright file="QueryBuilderRequest.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>04-12-2018</date>
// <summary>Class that helps to get the request from APIgateway</summary>
namespace ReportBuilder.Models.Request
{
    /// <summary>
    /// Class that helps to get the request from APIgateway
    /// </summary>
    public class QueryBuilderRequest
    {
        public List<EmployeeModel> Fields { get; set; }
        public string[] ColumnList { get; set; }

    }
}
