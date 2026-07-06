using System.Text;
using System.Text.Json;

namespace CodeQualityCoach.Reports
{
    public class ReportService : IReportService {
        public void GenerateAllReports(AnalysisReport report, string outputDirectory) {
            // создаем папку, если её нет
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
            
            string html = GenerateHtmlReport(report);
            SaveReport(html, Path.Combine(outputDirectory, "report.html"));
            
            string json = GenerateJsonReport(report);
            SaveReport(json, Path.Combine(outputDirectory, "report.json"));
            
            string text = GenerateTextReport(report);
            SaveReport(text, Path.Combine(outputDirectory, "report.txt"));
            // Console.WriteLine($" Отчеты сохранены в: {outputDirectory}");
        }

        // html
        public string GenerateHtmlReport(AnalysisReport report) {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8'>");
            sb.AppendLine("<title>Code Quality Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #2c3e50; }");
            sb.AppendLine(".summary { background: #ecf0f1; padding: 15px; border-radius: 8px; }");
            sb.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            sb.AppendLine("th { background: #34495e; color: white; padding: 10px; text-align: left; }");
            sb.AppendLine("td { padding: 10px; border-bottom: 1px solid #ddd; }");
            sb.AppendLine(".critical { color: #e74c3c; font-weight: bold; }");
            sb.AppendLine(".warning { color: #f39c12; font-weight: bold; }");
            sb.AppendLine(".info { color: #3498db; }");
            sb.AppendLine(".suggestion { background: #d5f5e3; padding: 8px; border-radius: 4px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // заголовок
            sb.AppendLine($"<h1>Отчет по качеству кода: {report.ProjectName}</h1>");
            sb.AppendLine($"<p><strong>Дата анализа:</strong> {report.AnalyzedAt:dd.MM.yyyy HH:mm}</p>");

            // сводка
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine($"<p><strong>Всего проблем:</strong> {report.TotalIssues}</p>");
            sb.AppendLine($"<p><strong>Критических:</strong> <span class='critical'>{report.CriticalIssues}</span></p>");
            sb.AppendLine($"<p><strong>Предупреждений:</strong> <span class='warning'>{report.Warnings}</span></p>");
            sb.AppendLine("</div>");

            // таблица с проблемами
            if (report.Issues.Any())
            {
                sb.AppendLine("<h2>Детали проблем</h2>");
                sb.AppendLine("<table>");
                sb.AppendLine("<tr><th>Файл</th><th>Строка</th><th>Важность</th><th>Правило</th><th>Описание</th><th>Исправление</th></tr>");

                foreach (var issue in report.Issues)
                {
                    string severityClass = issue.Severity.ToLower() switch
                    {
                        "critical" => "critical",
                        "warning" => "warning",
                        _ => "info"
                    };

                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td>{Path.GetFileName(issue.FilePath)}</td>");
                    sb.AppendLine($"<td>{issue.Line}</td>");
                    sb.AppendLine($"<td class='{severityClass}'>{issue.Severity}</td>");
                    sb.AppendLine($"<td>{issue.RuleId}</td>");
                    sb.AppendLine($"<td>{issue.Description}</td>");
                    sb.AppendLine($"<td><span class='suggestion'>{issue.Suggestion}</span></td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</table>");
            }
            else
            {
                sb.AppendLine("<h2>Проблем не найдено.</h2>");
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        // json
        public string GenerateJsonReport(AnalysisReport report) {
            var options = new JsonSerializerOptions {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(report, options);
        }

        // text
        public string GenerateTextReport(AnalysisReport report) {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"  ОТЧЕТ ПО КАЧЕСТВУ КОДА: {report.ProjectName}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"  Дата анализа: {report.AnalyzedAt:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"  Всего проблем: {report.TotalIssues}");
            sb.AppendLine($"  Критических: {report.CriticalIssues}");
            sb.AppendLine($"  Предупреждений: {report.Warnings}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine();

            if (!report.Issues.Any()) {
                sb.AppendLine("Проблем не найдено.");
                return sb.ToString();
            }

            int counter = 1;
            foreach (var issue in report.Issues) {
                sb.AppendLine($"[{counter}] {issue.RuleId} - {issue.Title}");
                sb.AppendLine($"    Файл: {issue.FilePath} (строка {issue.Line})");
                sb.AppendLine($"    Важность: {issue.Severity}");
                sb.AppendLine($"    Описание: {issue.Description}");
                sb.AppendLine($"    Исправление: {issue.Suggestion}");
                if (!string.IsNullOrEmpty(issue.CodeSnippet))
                {
                    sb.AppendLine($"    Код:");
                    sb.AppendLine($"      {issue.CodeSnippet.Replace("\n", "\n      ")}");
                }
                sb.AppendLine();
                counter++;
            }

            return sb.ToString();
        }
        
        public void SaveReport(string content, string filePath) {
            File.WriteAllText(filePath, content, Encoding.UTF8);
            Console.WriteLine($"Сохранено: {filePath}");
        }
    }
}