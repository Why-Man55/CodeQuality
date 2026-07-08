using System;
using System.Collections.Generic;
using System.Linq;

namespace Practice.Models {
    public class AnalysisReport {
        public string ProjectName { get; set; }
        public DateTime AnalyzedAt { get; set; }
        public List<Issue> Issues { get; set; } = new();
        public int TotalIssues => Issues.Count;
        public int CriticalIssues => Issues.Count(i => i.Severity == "Critical");
        public int Warnings => Issues.Count(i => i.Severity == "Warning");
    }
}