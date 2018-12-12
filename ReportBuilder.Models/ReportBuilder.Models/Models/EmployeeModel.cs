using System;
using System.Collections.Generic;
using System.Text;


/*
 <copyright file="EmployeeModel.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>26-11-2018</date>
 <summary>
    Model of field object for QueryBuilder employee request 
 </summary>
*/

namespace ReportBuilder.Models.Models
{
    public class EmployeeModel
    {
        public string Name { get; set; }

        public string Bitwise { get; set; }

        public string Value { get; set; }

        public string Operator { get; set; }

        public string Group { get; set; }
    }
}
