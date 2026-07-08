using System;
using System.Collections.Generic;
using Practice.Models;

namespace Practice.Helpers {
    public static class ReportDataHelper {
        public static AnalysisReport GetReportFromAnalyzers(string projectName)
        {
            // ЗДЕСЬ БУДУТ ВЫЗОВЫ АНАЛИЗАТОРОВ
            return new AnalysisReport
            {
                ProjectName = projectName,
                AnalyzedAt = DateTime.Now,
                Issues = new List<Issue>()
            };
        }
    }
}