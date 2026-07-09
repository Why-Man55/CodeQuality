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
            Title = ExtractTitle(ruleId),
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

    private static string ExtractTitle(string ruleId) =>
        ruleId switch
        {
            "ASYNC001" or "AsyncError" or "CQ001" => "Async void usage",
            "TASK001" or "ASYNC002" => "Synchronous Task wait",
            "ASYNC003" => "Missing CancellationToken",
            "Complexity" or "COMPLEX001" => "High cyclomatic complexity",
            "AntiPattern" or "CATCH001" => "Empty catch block",
            "METHOD001" => "Method is too large",
            "PARAM001" => "Too many method parameters",
            _ => "Code quality issue"
        };

    private static string ExtractRecommendation(string message)
    {
        var match = RecommendationRegex().Match(message);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static string DetermineSeverity(string message)
    {
        if (message.Contains("Error", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("TASK001", StringComparison.OrdinalIgnoreCase))
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

    [GeneratedRegex(@"(?:РЎС‚СЂРѕРєР°|Строка|Line):\s*(\d+)")]
    private static partial Regex LineRegex();

    [GeneratedRegex(@"(?:РСЃРїСЂР°РІР»РµРЅРёРµ|Исправление|Fix):\s*(.+)")]
    private static partial Regex RecommendationRegex();
}
