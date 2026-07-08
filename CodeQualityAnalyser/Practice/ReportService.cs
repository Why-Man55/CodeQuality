using CodeQualityAnalyser.Practice.Models;
using CodeQualityAnalyser.Practice.ReportGenerators;

namespace CodeQualityAnalyser.Practice
{
    public class ReportService : IReportService {
        private readonly IEnumerable<IReportGenerator> _reportGenerators;
        private readonly FileReportWriter _fileWriter;

        public ReportService() {
            _reportGenerators = new List<IReportGenerator>
            {
                new HtmlReportGenerator(),
                new JsonReportGenerator(),
                new TxtReportGenerator() 
            };
            _fileWriter = new FileReportWriter();
        }

        public ReportService(IEnumerable<IReportGenerator> reportGenerators, FileReportWriter fileWriter) {
            _reportGenerators = reportGenerators ?? throw new ArgumentNullException(nameof(reportGenerators));
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public async Task GenerateAllReportsAsync(AnalysisReport report, string outputDirectory) {
            if(report == null)
                throw new ArgumentNullException(nameof(report), "Отчет не может быть null");

            if(string.IsNullOrEmpty(outputDirectory))
                throw new ArgumentException("Путь к папке не может быть пустым", nameof(outputDirectory));

            if(!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            foreach(var generator in _reportGenerators) {
                string content = generator.Generate(report);
                string fileName = $"report.{generator.GetFileExtension()}";
                string filePath = Path.Combine(outputDirectory, fileName);
                await _fileWriter.SaveReportAsync(content, filePath);
            }

            Console.WriteLine($"Отчеты сохранены в: {outputDirectory}");
        }
    }
}