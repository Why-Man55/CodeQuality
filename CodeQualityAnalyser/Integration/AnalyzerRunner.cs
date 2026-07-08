using CodeQualityAnalyser.AnalysServices;

using CodeQualityAnalyser.Roslyn;

using CodeQualityAnalyser.Models;

using Microsoft.CodeAnalysis;



namespace CodeQualityAnalyser.Integration;



public sealed class AnalyzerRunner

{

    private readonly RoslynEngine _roslynEngine;

    private readonly IReadOnlyList<IAnalyser> _analysers;

    private readonly IssueEnricher _issueEnricher;



    public AnalyzerRunner(

        RoslynEngine roslynEngine,

        IEnumerable<IAnalyser> analysers,

        IssueEnricher issueEnricher)

    {

        _roslynEngine = roslynEngine;

        _analysers = analysers.ToList();

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



            foreach (var analyser in _analysers)

            {

                foreach (var message in analyser.GetAnalysis(tree))

                {

                    issues.Add(IssueMapper.ToDto(message, fileName));

                }

            }



            var orchestratorResults = await Orchestrator.startTest(tree.GetRoot());

            for (var i = 0; i < orchestratorResults.Length; i++)

            {

                foreach (var result in orchestratorResults[i])

                {

                    if (OrchestratorIssueMapper.TryMap(i, result, fileName, out var mappedIssue))

                    {

                        issues.Add(mappedIssue);

                    }

                    else

                    {

                        issues.Add(IssueMapper.ToDto(result, fileName));

                    }

                }

            }

        }



        return issues.Select(_issueEnricher.Enrich).ToList();

    }

}


