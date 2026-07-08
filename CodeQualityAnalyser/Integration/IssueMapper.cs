using System.Text.RegularExpressions;
using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Integration;

public static partial class IssueMapper
{
    public static IssueDto ToDto(string message, string? sourceFilePath = null)
    {
        var ruleId = ExtractRuleId(message);
        var filePath = ExtractFilePath(message) ?? sourceFilePath ?? string.Empty;
        var line = ExtractLine(message);

        return new IssueDto
        {
            RuleId = ruleId,
            Title = ExtractTitle(message, ruleId),
            Description = message,
            Recommendation = ExtractRecommendation(message),
            Severity = DetermineSeverity(message),
            FilePath = filePath,
            Line = line,
            Column = 0
        };
    }

    private static string ExtractRuleId(string message)
    {
        var match = RuleIdRegex().Match(message);
        return match.Success ? match.Groups[1].Value : "CQ000";
    }

    private static string? ExtractFilePath(string message)
    {
        var match = FilePathRegex().Match(message);
        return match.Success ? match.Groups[1].Value : null;
    }

    private static int ExtractLine(string message)
    {
        var match = LineRegex().Match(message);
        return match.Success && int.TryParse(match.Groups[1].Value, out var line) ? line : 0;
    }

    private static string ExtractTitle(string message, string ruleId) =>
        ruleId switch
        {
            "ASYNC001" or "AsyncError" or "CQ001" => "Использование async void",
            "TASK001" or "ASYNC002" => "Синхронное ожидание Task",
            "ASYNC003" => "Отсутствует CancellationToken",
            "Complexity" or "COMPLEX001" => "Высокая цикломатическая сложность",
            "AntiPattern" or "CATCH001" => "Пустой блок catch",
            "METHOD001" => "Слишком большой метод",
            "PARAM001" => "Слишком много параметров",
            _ => "Обнаружена проблема"
        };

    private static string ExtractRecommendation(string message)
    {
        var match = RecommendationRegex().Match(message);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static string DetermineSeverity(string message)
    {
        if (message.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("TASK001", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("⛔", StringComparison.OrdinalIgnoreCase))
        {
            return "Error";
        }

        if (message.Contains("Info", StringComparison.OrdinalIgnoreCase))
        {
            return "Info";
        }

        return "Warning";
    }

    [GeneratedRegex(@"\[([A-Za-z0-9]+)\]")]
    private static partial Regex RuleIdRegex();

    [GeneratedRegex(@"(?:^|\s)([\w./\\-]+\.cs):")]
    private static partial Regex FilePathRegex();

    [GeneratedRegex(@"Строка:\s*(\d+)")]
    private static partial Regex LineRegex();

    [GeneratedRegex(@"Исправление:\s*(.+)")]
    private static partial Regex RecommendationRegex();
}
