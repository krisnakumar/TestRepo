﻿using System;
using System.Collections.Generic;
using System.Text;


/*
 <copyright file="ErrorResponse.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    Response model for error / bad request / forbidden / unauthorized status
 </summary>
*/

namespace ReportBuilder.Models.Response
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
