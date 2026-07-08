using CodeQualityAnalyser.AnalysServices;
using CodeQualityAnalyser.Explanation;
using CodeQualityAnalyser.Integration;
using CodeQualityAnalyser.Models;
using CodeQualityAnalyser.Practice.Models;
using CodeQualityAnalyser.Roslyn;

namespace CodeQualityAnalyser.Practice.Helpers;

public static class ReportDataHelper
{
    public static AnalysisReport GetReportFromAnalyzers(string projectName, string? sourceDirectory = null)
    {
        var directory = string.IsNullOrWhiteSpace(sourceDirectory)
            ? Directory.GetCurrentDirectory()
            : sourceDirectory;

        var csharpFiles = SolutionExtractor.FindCSharpFiles(directory);
        if (csharpFiles.Count == 0)
        {
            return new AnalysisReport
            {
                ProjectName = projectName,
                AnalyzedAt = DateTime.Now,
                Issues = []
            };
        }

        var roslynEngine = new RoslynEngine();
        var enricher = new IssueEnricher(new ExplanationGenerator());
        var runner = new AnalyzerRunner(
            roslynEngine,
            new IAnalyser[]
            {
                new AsyncVoidAnalyser(),
                new ComplexityAnalyser(),
                new EmptyCatchAnalyser()
            },
            enricher);

        var issueDtos = runner.RunAndMapAsync(csharpFiles, CancellationToken.None).GetAwaiter().GetResult();

        return new AnalysisReport
        {
            ProjectName = projectName,
            AnalyzedAt = DateTime.Now,
            Issues = issueDtos.Select(MapToPracticeIssue).ToList()
        };
    }

    private static Issue MapToPracticeIssue(IssueDto dto)
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
