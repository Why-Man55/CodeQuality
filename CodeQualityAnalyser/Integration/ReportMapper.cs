using CodeQualityAnalyser.Models;
using CodeQualityAnalyser.Practice.Models;

namespace CodeQualityAnalyser.Integration;

public static class ReportMapper
{
    public static AnalysisReport ToReport(AnalysisResultDto result)
    {
        return new AnalysisReport
        {
            ProjectName = result.ProjectName,
            AnalyzedAt = result.AnalyzedAt,
            Issues = result.Issues.Select(ToPracticeIssue).ToList()
        };
    }

    private static Issue ToPracticeIssue(IssueDto dto)
    {
        return new Issue
        {
            RuleId = dto.RuleId,
            Title = dto.Title,
            Description = dto.Description,
            Suggestion = dto.Recommendation,
            Severity = dto.Severity,
            FilePath = dto.FilePath,
            Line = dto.Line,
            CodeSnippet = string.Empty
        };
    }
}
