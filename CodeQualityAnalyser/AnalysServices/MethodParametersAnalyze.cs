using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class MethodParametersAnalyze : IAnalyser
{
    private const int MaxParameterCount = 4;

    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            var actualParameterCount = method.ParameterList.Parameters.Count;

            if (actualParameterCount > MaxParameterCount)
            {
                var lineNumber = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                errors.Add(
                    $"[PARAM001] {fileName}: Method '{method.Identifier.Text}' has more than {MaxParameterCount} parameters. " +
                    $"Line: {lineNumber}. Fix: Reduce the parameter list or group related values into a separate type.");
            }
        }

        return errors;
    }
}
