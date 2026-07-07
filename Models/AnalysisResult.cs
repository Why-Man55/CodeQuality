using System;

namespace CodeQualityCoach.Core.Models
{
    public class AnalysisResult
    {
        public string FilePath { get; set; }
        public string DiagnosticId { get; set; }
        public string Severity { get; set; }
        public string Location { get; set; }
        public string CodeSnippet { get; set; }
        public string Message { get; set; }

        public AnalysisResult()
        {
            FilePath = string.Empty;
            DiagnosticId = string.Empty;
            Severity = string.Empty;
            Location = string.Empty;
            CodeSnippet = string.Empty;
            Message = string.Empty;
        }
    }
}