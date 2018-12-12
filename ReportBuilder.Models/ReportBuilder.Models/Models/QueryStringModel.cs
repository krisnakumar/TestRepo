using System;
using System.Collections.Generic;
using System.Text;


/*
 <copyright file="QueryStringModel.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>02-11-2018</date>
 <summary>
    Model for workbook request object
 </summary>
*/

namespace ReportBuilder.Models.Models
{
    public class QueryStringModel
    {
        public int CompletedWorkBooks {get;set;}

        public int WorkBookInDue { get; set; }

        public int PastDueWorkBook { get; set; }

        public string Param { get; set; }

        public string CompletedWorkBook { get; set; }

        public string WorkBooksInDue { get; set; }

        public string PastDueWorkBooks { get; set; }

    }
}
