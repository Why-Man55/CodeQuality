using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser.Integration;

internal sealed class CancellationTokenAnalyzeAdapter : Analyze
{
    private readonly CancellationTokenAnalyze _analyzer = new();

    public override string[] startTest(SyntaxNode root) =>
        _analyzer.startTest(root.SyntaxTree.GetText().ToString());
}

internal sealed class TaskResultAnalyzerAdapter : Analyze
{
    private readonly TaskResultAnalyzer _analyzer = new();

    public override string[] startTest(SyntaxNode root) =>
        _analyzer.startTest(root.SyntaxTree.GetText().ToString());
}
