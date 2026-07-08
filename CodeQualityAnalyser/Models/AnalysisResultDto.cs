namespace CodeQualityAnalyser.Models;

public sealed class AnalysisResultDto
{
    public string ProjectName { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
    public IReadOnlyList<IssueDto> Issues { get; set; } = [];
    public int TotalIssues { get; set; }
}
