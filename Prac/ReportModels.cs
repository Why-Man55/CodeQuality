namespace CodeQualityCoach.Reports
{
    public class AnalysisReport {
        public string ProjectName { get; set; }
        public DateTime AnalyzedAt { get; set; }
        public List<Issue> Issues { get; set; }=new();
        public int TotalIssues => Issues.Count;
        public int CriticalIssues => Issues.Count(i => i.Severity == "Critical");
        public int Warnings => Issues.Count(i => i.Severity == "Warning");
    }

    // модель ошибки
    public class Issue {
        public string FilePath { get; set; }
        public int Line { get; set; }
        public string Severity { get; set; } // Critical, Warning, Info
        public string RuleId { get; set; }   // ID
        public string Title { get; set; }    // название
        public string Description { get; set; } // объяснение
        public string Suggestion { get; set; }  // как исправить
        public string CodeSnippet { get; set; }  // фрагмент кода с проблемой
    }
}