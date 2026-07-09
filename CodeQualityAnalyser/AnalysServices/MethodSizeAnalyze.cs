using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class MethodSizeAnalyze : IAnalyser
{
    private const int MaxLineCount = 30;

    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            if (method.Body == null)
            {
                continue;
            }

            var lineSpan = method.Body.GetLocation().GetLineSpan();
            var methodLines = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line - 1;

            if (methodLines > MaxLineCount)
            {
                var lineNumber = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                errors.Add(
                    $"[METHOD001] {fileName}: Method '{method.Identifier.Text}' contains more than {MaxLineCount} lines. " +
                    $"Line: {lineNumber}. Fix: Split the method into smaller methods with clear responsibilities.");
            }
        }

        return errors;
    }
}
