using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class CancellationTokenAnalyze : IAnalyser
{
    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();
        var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methodDeclarations)
        {
            if (!IsAsyncMethod(method) || HasCancellationTokenParameter(method))
            {
                continue;
            }

            var lineNumber = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            errors.Add(
                $"[ASYNC003] {fileName}: Async method '{method.Identifier.Text}' does not accept CancellationToken. " +
                $"Line: {lineNumber}. Fix: Add CancellationToken cancellationToken = default and pass it to async calls.");
        }

        return errors;
    }

    private static bool IsAsyncMethod(MethodDeclarationSyntax method) =>
        method.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword));

    private static bool HasCancellationTokenParameter(MethodDeclarationSyntax method) =>
        method.ParameterList.Parameters.Any(p =>
            p.Type != null &&
            (p.Type.ToString() == "CancellationToken" ||
             p.Type.ToString() == "System.Threading.CancellationToken"));
}
