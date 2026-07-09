using CodeQualityAnalyser.Models;
using CodeQualityAnalyser.Roslyn;

namespace CodeQualityAnalyser.Integration;

public sealed class AnalyzerRunner
{
    private readonly RoslynEngine _roslynEngine;
    private readonly IssueEnricher _issueEnricher;

    public AnalyzerRunner(
        RoslynEngine roslynEngine,
        IssueEnricher issueEnricher)
    {
        _roslynEngine = roslynEngine;
        _issueEnricher = issueEnricher;
    }

    public async Task<List<IssueDto>> RunAndMapAsync(IEnumerable<string> csFilePaths, CancellationToken cancellationToken)
    {
        var filePaths = csFilePaths.ToList();
        if (filePaths.Count == 0)
        {
            return [];
        }

        var syntaxTrees = await _roslynEngine.GetSyntaxTreesAsync(filePaths);
        var issues = new List<IssueDto>();

        foreach (var tree in syntaxTrees)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fileName = Path.GetFileName(tree.FilePath);
            var results = await Orchestrator.startTest(tree);

            foreach (var message in results)
            {
                issues.Add(IssueMapper.ToDto(message, fileName));
            }
        }

        return issues.Select(_issueEnricher.Enrich).ToList();
    }
}
