using CodeQualityAnalyser.Practice.Models;

namespace CodeQualityAnalyser.Practice.ReportGenerators {
    public interface IReportGenerator {
        string Generate(AnalysisReport report);
        string GetFileExtension();
    }
}