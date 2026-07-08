using System;
using System.Text.Json;
using Practice.Models;

namespace Practice.ReportGenerators {
    public class JsonReportGenerator : IReportGenerator {
        public string Generate(AnalysisReport report) {
            if (report == null)
                throw new ArgumentNullException(nameof(report), "Отчет не может быть null");

            var options = new JsonSerializerOptions {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(report, options);
        }

        public string GetFileExtension() => "json";
    }
}