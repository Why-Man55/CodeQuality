using System;
using System.IO;
using System.Threading.Tasks;
using Practice;
using Practice.Helpers;
using Practice.Models; 

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string projectName = args.Length > 0 ? args[0] : "Мой проект";

            // Получаем данные от анализаторов
            AnalysisReport report = ReportDataHelper.GetReportFromAnalyzers(projectName);
            IReportService reportService = new ReportService();
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            await reportService.GenerateAllReportsAsync(report, outputDir);

            Console.WriteLine($"\nПапка с отчетами: {outputDir}");
            Console.WriteLine($"Всего найдено проблем: {report.TotalIssues}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}