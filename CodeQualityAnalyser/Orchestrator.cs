using CodeQualityAnalyser.AnalysServices;
using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser;

public class Orchestrator
{
    private static readonly IAnalyser[] Tests =
    [
        new AsyncVoidAnalyser(),
        new ComplexityAnalyser(),
        new EmptyCatchAnalyser(),
        new MethodSizeAnalyze(),
        new MethodParametersAnalyze(),
        new CancellationTokenAnalyze(),
        new TaskResultAnalyzer()
    ];

    public static async Task<List<string>> startTest(SyntaxTree tree)
    {
        var tasks = new Task<List<string>>[Tests.Length];

        for (var i = 0; i < Tests.Length; i++)
        {
            var index = i;

            tasks[index] = Task.Run(() =>
            {
                try
                {
                    return Tests[index].GetAnalysis(tree);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Analyzer {Tests[index].GetType().Name} failed: {ex.Message}");
                    return [];
                }
            });
        }

        var results = await Task.WhenAll(tasks);
        return results.SelectMany(result => result).ToList();
    }
}
