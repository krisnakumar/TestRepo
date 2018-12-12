
/*
 <copyright file="AttemptsResponse.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>22-10-2018</date>
 <summary>
    Model for task attempts response object
 </summary>
*/

namespace ReportBuilder.Models.Response
{
    public class AttemptsResponse
    {
        public string Attempt { get; set; }
        public string Status { get; set; }
        public string DateTime { get; set; }
        public string Location { get; set; }

        public string Evaluator { get; set; }

        public string Comments { get; set; }

    }
}
