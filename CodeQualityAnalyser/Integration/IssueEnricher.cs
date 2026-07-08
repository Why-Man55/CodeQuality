using CodeQualityAnalyser.Explanation;
using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Integration;

public sealed partial class IssueEnricher
{
    private readonly IExplanationGenerator _explanationGenerator;

    public IssueEnricher(IExplanationGenerator explanationGenerator)
    {
        _explanationGenerator = explanationGenerator;
    }

    public IssueDto Enrich(IssueDto issue)
    {
        var normalizedRuleId = RuleIdNormalizer.Normalize(issue.RuleId);
        var analysisResult = new AnalysisResult
        {
            DiagnosticId = normalizedRuleId,
            FilePath = issue.FilePath,
            Severity = issue.Severity,
            Location = issue.Line > 0 ? issue.Line.ToString() : string.Empty,
            CodeSnippet = ExtractCodeSnippet(issue.Description),
            Message = issue.Description
        };

        var explanation = _explanationGenerator.Generate(analysisResult);

        return new IssueDto
        {
            RuleId = normalizedRuleId,
            Title = IsGenericTitle(issue.Title) ? CleanTitle(explanation.Title) : issue.Title,
            Description = issue.Description,
            Recommendation = string.IsNullOrWhiteSpace(issue.Recommendation)
                ? explanation.Recommendation
                : issue.Recommendation,
            Severity = issue.Severity,
            FilePath = issue.FilePath,
            Line = issue.Line,
            Column = issue.Column
        };
    }

    private static bool IsGenericTitle(string title) =>
        string.IsNullOrWhiteSpace(title) || title == "Обнаружена проблема";

    private static string CleanTitle(string title)
    {
        return title
            .Replace("⚠️ ", string.Empty)
            .Replace("⛔ ", string.Empty)
            .Replace("🧵 ", string.Empty)
            .Replace("🧩 ", string.Empty)
            .Replace("📌 ", string.Empty)
            .Replace("📏 ", string.Empty)
            .Replace("📋 ", string.Empty)
            .Trim();
    }

    private static string ExtractCodeSnippet(string description)
    {
        var methodMatch = MethodNameRegex().Match(description);
        if (methodMatch.Success)
        {
            return methodMatch.Groups[1].Value;
        }

        var codeMatch = CodeSnippetRegex().Match(description);
        return codeMatch.Success ? codeMatch.Groups[1].Value.Trim() : description;
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"'([^']+)'")]
    private static partial System.Text.RegularExpressions.Regex MethodNameRegex();

    [System.Text.RegularExpressions.GeneratedRegex(@":\s*([^|]+)")]
    private static partial System.Text.RegularExpressions.Regex CodeSnippetRegex();
}
