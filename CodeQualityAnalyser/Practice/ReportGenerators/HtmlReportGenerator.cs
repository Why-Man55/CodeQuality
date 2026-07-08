using System.Text;
using CodeQualityAnalyser.Practice.Models;

namespace CodeQualityAnalyser.Practice.ReportGenerators
{
    public class HtmlReportGenerator : IReportGenerator {
        public string Generate(AnalysisReport report) {
            if (report == null)
                throw new ArgumentNullException(nameof(report), "Отчет не может быть null");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='ru'>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8'>");
            sb.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            sb.AppendLine("<title>Code Quality Report</title>");
            sb.AppendLine("<style>");
            
            // стили
            sb.AppendLine("  * { margin: 0; padding: 0; box-sizing: border-box; }");
            sb.AppendLine("  body {");
            sb.AppendLine("      font-family: 'Segoe UI', Arial, sans-serif;");
            sb.AppendLine("      background: #f0f2f5;");
            sb.AppendLine("      padding: 40px 20px;");
            sb.AppendLine("      display: flex;");
            sb.AppendLine("      justify-content: center;");
            sb.AppendLine("  }");
            sb.AppendLine("  .container {");
            sb.AppendLine("      max-width: 1100px;");
            sb.AppendLine("      width: 100%;");
            sb.AppendLine("      background: white;");
            sb.AppendLine("      border-radius: 16px;");
            sb.AppendLine("      box-shadow: 0 8px 30px rgba(0,0,0,0.12);");
            sb.AppendLine("      padding: 40px;");
            sb.AppendLine("  }");
            
            // зголовок
            sb.AppendLine("  .header {");
            sb.AppendLine("      display: flex;");
            sb.AppendLine("      align-items: center;");
            sb.AppendLine("      gap: 14px;");
            sb.AppendLine("      margin-bottom: 8px;");
            sb.AppendLine("  }");
            sb.AppendLine("  .header-icon { font-size: 32px; }");
            sb.AppendLine("  h1 {");
            sb.AppendLine("      font-size: 26px;");
            sb.AppendLine("      color: #1a1a2e;");
            sb.AppendLine("      font-weight: 700;");
            sb.AppendLine("  }");
            sb.AppendLine("  .subtitle {");
            sb.AppendLine("      color: #6b7280;");
            sb.AppendLine("      font-size: 15px;");
            sb.AppendLine("      margin-bottom: 28px;");
            sb.AppendLine("      border-bottom: 2px solid #e5e7eb;");
            sb.AppendLine("      padding-bottom: 16px;");
            sb.AppendLine("  }");
            
            // сводка
            sb.AppendLine("  .summary {");
            sb.AppendLine("      display: flex;");
            sb.AppendLine("      gap: 20px;");
            sb.AppendLine("      flex-wrap: wrap;");
            sb.AppendLine("      margin-bottom: 32px;");
            sb.AppendLine("  }");
            sb.AppendLine("  .summary-card {");
            sb.AppendLine("      flex: 1;");
            sb.AppendLine("      min-width: 140px;");
            sb.AppendLine("      background: #f8fafc;");
            sb.AppendLine("      border-radius: 12px;");
            sb.AppendLine("      padding: 18px 22px;");
            sb.AppendLine("      text-align: center;");
            sb.AppendLine("      border: 1px solid #e5e7eb;");
            sb.AppendLine("      transition: transform 0.2s;");
            sb.AppendLine("  }");
            sb.AppendLine("  .summary-card:hover { transform: translateY(-2px); }");
            sb.AppendLine("  .summary-number {");
            sb.AppendLine("      font-size: 32px;");
            sb.AppendLine("      font-weight: 700;");
            sb.AppendLine("      color: #1a1a2e;");
            sb.AppendLine("  }");
            sb.AppendLine("  .summary-label {");
            sb.AppendLine("      font-size: 14px;");
            sb.AppendLine("      color: #6b7280;");
            sb.AppendLine("      margin-top: 4px;");
            sb.AppendLine("  }");
            sb.AppendLine("  .critical-color { color: #dc2626; }");
            sb.AppendLine("  .warning-color { color: #f59e0b; }");
            sb.AppendLine("  .info-color { color: #3b82f6; }");
            
            // таблица
            sb.AppendLine("  h2 {");
            sb.AppendLine("      font-size: 20px;");
            sb.AppendLine("      color: #1a1a2e;");
            sb.AppendLine("      margin-bottom: 16px;");
            sb.AppendLine("      display: flex;");
            sb.AppendLine("      align-items: center;");
            sb.AppendLine("      gap: 10px;");
            sb.AppendLine("  }");
            sb.AppendLine("  table {");
            sb.AppendLine("      width: 100%;");
            sb.AppendLine("      border-collapse: collapse;");
            sb.AppendLine("      border-radius: 12px;");
            sb.AppendLine("      overflow: hidden;");
            sb.AppendLine("      box-shadow: 0 1px 3px rgba(0,0,0,0.05);");
            sb.AppendLine("  }");
            sb.AppendLine("  th {");
            sb.AppendLine("      background: #1a1a2e;");
            sb.AppendLine("      color: white;");
            sb.AppendLine("      padding: 14px 16px;");
            sb.AppendLine("      text-align: left;");
            sb.AppendLine("      font-weight: 600;");
            sb.AppendLine("      font-size: 14px;");
            sb.AppendLine("  }");
            sb.AppendLine("  td {");
            sb.AppendLine("      padding: 13px 16px;");
            sb.AppendLine("      border-bottom: 1px solid #e5e7eb;");
            sb.AppendLine("      font-size: 14px;");
            sb.AppendLine("      vertical-align: top;");
            sb.AppendLine("  }");
            sb.AppendLine("  tr:hover td { background: #f8fafc; }");
            sb.AppendLine("  .critical { color: #dc2626; font-weight: 600; }");
            sb.AppendLine("  .warning { color: #f59e0b; font-weight: 600; }");
            sb.AppendLine("  .info { color: #3b82f6; font-weight: 600; }");
            sb.AppendLine("  .suggestion {");
            sb.AppendLine("      background: #ecfdf5;");
            sb.AppendLine("      padding: 6px 10px;");
            sb.AppendLine("      border-radius: 6px;");
            sb.AppendLine("      display: inline-block;");
            sb.AppendLine("      font-size: 13px;");
            sb.AppendLine("      color: #065f46;");
            sb.AppendLine("  }");
            
            // "нет проблем"
            sb.AppendLine("  .success-box {");
            sb.AppendLine("      background: #ecfdf5;");
            sb.AppendLine("      border: 2px solid #10b981;");
            sb.AppendLine("      border-radius: 12px;");
            sb.AppendLine("      padding: 40px;");
            sb.AppendLine("      text-align: center;");
            sb.AppendLine("      margin-top: 12px;");
            sb.AppendLine("  }");
            sb.AppendLine("  .success-box .emoji { font-size: 48px; display: block; margin-bottom: 12px; }");
            sb.AppendLine("  .success-box h2 {");
            sb.AppendLine("      color: #065f46;");
            sb.AppendLine("      justify-content: center;");
            sb.AppendLine("      margin-bottom: 0;");
            sb.AppendLine("  }");
            
            sb.AppendLine("  .footer {");
            sb.AppendLine("      margin-top: 32px;");
            sb.AppendLine("      text-align: center;");
            sb.AppendLine("      color: #9ca3af;");
            sb.AppendLine("      font-size: 13px;");
            sb.AppendLine("      border-top: 1px solid #e5e7eb;");
            sb.AppendLine("      padding-top: 20px;");
            sb.AppendLine("  }");
            
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            
            
            sb.AppendLine("<div class='container'>");
            
            // заголовок
            sb.AppendLine("<div class='header'>");
            sb.AppendLine("<span class='header-icon'>📊</span>");
            sb.AppendLine($"<h1>Отчет по качеству кода</h1>");
            sb.AppendLine("</div>");
            sb.AppendLine($"<div class='subtitle'>📁 {report.ProjectName} &nbsp;·&nbsp; 🕐 {report.AnalyzedAt:dd.MM.yyyy HH:mm}</div>");
            
            // карточки
            sb.AppendLine("<div class='summary'>");
            
            sb.AppendLine("<div class='summary-card'>");
            sb.AppendLine($"<div class='summary-number'>{report.TotalIssues}</div>");
            sb.AppendLine("<div class='summary-label'>Всего проблем</div>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<div class='summary-card'>");
            sb.AppendLine($"<div class='summary-number critical-color'>{report.CriticalIssues}</div>");
            sb.AppendLine("<div class='summary-label'>🔴 Критических</div>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<div class='summary-card'>");
            sb.AppendLine($"<div class='summary-number warning-color'>{report.Warnings}</div>");
            sb.AppendLine("<div class='summary-label'>⚠️ Предупреждений</div>");
            sb.AppendLine("</div>");
            
            int infoCount = report.TotalIssues - report.CriticalIssues - report.Warnings;
            sb.AppendLine("<div class='summary-card'>");
            sb.AppendLine($"<div class='summary-number info-color'>{infoCount}</div>");
            sb.AppendLine("<div class='summary-label'>ℹ️ Информационных</div>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("</div>");
            
            if (report.Issues.Any())
            {
                sb.AppendLine("<h2>🔍 Детали проблем</h2>");
                sb.AppendLine("<table>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th>Файл</th>");
                sb.AppendLine("<th>Стр.</th>");
                sb.AppendLine("<th>Важность</th>");
                sb.AppendLine("<th>Правило</th>");
                sb.AppendLine("<th>Описание</th>");
                sb.AppendLine("<th>Исправление</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");

                foreach (var issue in report.Issues)
                {
                    string severityClass = issue.Severity.ToLower() switch
                    {
                        "critical" => "critical",
                        "warning" => "warning",
                        _ => "info"
                    };

                    string severityIcon = issue.Severity.ToLower() switch
                    {
                        "critical" => "🔴",
                        "warning" => "⚠️",
                        _ => "ℹ️"
                    };

                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td><strong>{Path.GetFileName(issue.FilePath)}</strong></td>");
                    sb.AppendLine($"<td>{issue.Line}</td>");
                    sb.AppendLine($"<td><span class='{severityClass}'>{severityIcon} {issue.Severity}</span></td>");
                    sb.AppendLine($"<td><code style='background:#f1f3f4;padding:2px 8px;border-radius:4px;font-size:13px;'>{issue.RuleId}</code></td>");
                    sb.AppendLine($"<td>{issue.Description}</td>");
                    sb.AppendLine($"<td><span class='suggestion'>💡 {issue.Suggestion}</span></td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }
            else
            {
                sb.AppendLine("<div class='success-box'>");
                sb.AppendLine("<span class='emoji'>🎉</span>");
                sb.AppendLine("<h2>Отлично! Проблем не найдено.</h2>");
                sb.AppendLine("<p style='color:#6b7280;margin-top:8px;font-size:16px;'>Ваш код соответствует всем правилам качества</p>");
                sb.AppendLine("</div>");
            }
            
            // Подвал
            sb.AppendLine("<div class='footer'>");
            sb.AppendLine($"{report.AnalyzedAt:dd.MM.yyyy HH:mm} · Code Quality Coach");
            sb.AppendLine("</div>");
            
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public string GetFileExtension() => "html";
    }
}


/*
using System;
using System.IO;
using System.Linq;
using System.Text;
using Practice.Models;

namespace Practice.ReportGenerators
{
    public class HtmlReportGenerator : IReportGenerator
    {
        public string Generate(AnalysisReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report), "Отчет не может быть null");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8'>");
            sb.AppendLine("<title>Code Quality Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #2c3e50; }");
            sb.AppendLine(".summary { padding: 15px; border-radius: 8px; border: 2px solid #34495e; }");
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

            sb.AppendLine($"<h1>Отчет по качеству кода: {report.ProjectName}</h1>");
            sb.AppendLine($"<p><strong>Дата анализа:</strong> {report.AnalyzedAt:dd.MM.yyyy HH:mm}</p>");

            sb.AppendLine("<div class='summary'>");
            sb.AppendLine($"<p><strong>Всего проблем:</strong> {report.TotalIssues}</p>");
            sb.AppendLine($"<p><strong>Критических:</strong> <span class='critical'>{report.CriticalIssues}</span></p>");
            sb.AppendLine($"<p><strong>Предупреждений:</strong> <span class='warning'>{report.Warnings}</span></p>");
            sb.AppendLine("</div>");

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
                sb.AppendLine("<h2>Отлично! Проблем не найдено.</h2>");
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public string GetFileExtension() => "html";
    }
}
*/