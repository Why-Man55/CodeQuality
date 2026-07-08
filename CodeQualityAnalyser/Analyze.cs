using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser;

public abstract class Analyze
{
    public virtual string[] startTest(SyntaxNode root) => Array.Empty<string>();

    public virtual string[] startTest(string fileContent) => Array.Empty<string>();
}
