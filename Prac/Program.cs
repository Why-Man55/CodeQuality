using CodeQualityCoach.Reports;

class Program
{
    static void Main(string[] args)
    {
        // отчет от анализаторов
        AnalysisReport report = GetReportFromAnalyzers();

        // создаем сервис отчетов
        IReportService reportService = new ReportService();
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

        // генерируем отчеты
        reportService.GenerateAllReports(report, outputDir);
        Console.WriteLine($"\n Отчеты сохранены в: {outputDir}");
    }

    static AnalysisReport GetReportFromAnalyzers()
    {
        // вызов твоих анализаторов
        return new AnalysisReport
        {
            ProjectName = "CodeQualityCoach", 
            AnalyzedAt = DateTime.Now,
            Issues = new List<Issue>() // список проблем, собранных анализаторами
        };
    }
}