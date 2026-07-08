using System;
using System.Text;
using Practice.Models;

namespace Practice.ReportGenerators 
{
    public class TxtReportGenerator : IReportGenerator {
        public string Generate(AnalysisReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report), "Отчет не может быть null");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"  Отчет по качеству кода: {report.ProjectName}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"  Дата анализа: {report.AnalyzedAt:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"  Всего проблем: {report.TotalIssues}");
            sb.AppendLine($"  Критических: {report.CriticalIssues}");
            sb.AppendLine($"  Предупреждений: {report.Warnings}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine();

            if (!report.Issues.Any()) {
                sb.AppendLine("Отлично! Проблем не найдено.");
                return sb.ToString();
            }

            int counter = 1;
            foreach (var issue in report.Issues)
            {
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

        public string GetFileExtension() => "txt";
    }
}