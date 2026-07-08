using Practice.Models;

namespace Practice.ReportGenerators {
    public interface IReportGenerator {
        string Generate(AnalysisReport report);
        string GetFileExtension();
    }
}