namespace CodeQualityCoach.Reports
{
    public interface IReportService {
        void GenerateAllReports(AnalysisReport report, string outputDirectory);
        string GenerateHtmlReport(AnalysisReport report); // html
        string GenerateJsonReport(AnalysisReport report); //json
        string GenerateTextReport(AnalysisReport report); // text
        void SaveReport(string content, string filePath);
    }
}