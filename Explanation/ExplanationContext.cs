using System;
using System.Collections.Generic;
using CodeQualityCoach.Core.Models;

namespace CodeQualityCoach.Core.Explanation
{
    public class ExplanationContext
    {
        public List<AnalysisResult> AllResults { get; set; }
        public string ProjectName { get; set; }
        public string FrameworkVersion { get; set; }

        public ExplanationContext()
        {
            AllResults = new List<AnalysisResult>();
            ProjectName = string.Empty;
            FrameworkVersion = string.Empty;
        }

        public int CountByDiagnostic(string id)
        {
            int count = 0;
            foreach (var result in AllResults)
            {
                if (result.DiagnosticId == id)
                {
                    count++;
                }
            }
            return count;
        }
    }
}