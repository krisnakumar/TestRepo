namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for helping with workbook reporting, details on workbook content
    /// </summary>
    public class WorkbookReportEntity
    {
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        public int EntityType { get; set; }
        public string WorkbookEntityStatus { get; set; }
        public string FirstAttemptDate { get; set; }
        public string LastAttemptDate { get; set; }
        public int? LastSignOffById { get; set; }
        public string LastSignOffByName { get; set; }
        public int Repetitions { get; set; }
        public int NumberCompleted { get; set; }
        public int RemainingReps { get; set; }
        public string CompletionStatus { get; set; }
    }
}