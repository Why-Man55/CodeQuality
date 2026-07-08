using CodeQualityAnalyser.Practice.Models;

namespace CodeQualityAnalyser.Practice
{
    public interface IReportService {
        Task GenerateAllReportsAsync(AnalysisReport report, string outputDirectory);
    }
}