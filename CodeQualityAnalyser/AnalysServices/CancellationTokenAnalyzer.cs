using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser;

public class CancellationTokenAnalyze : Analyze
{
    public string[] startTest(string fileContent)
    {
        var invalidMethods = new List<string>();

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
        var root = syntaxTree.GetRoot();

        var methodDeclarations = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var method in methodDeclarations)
        {
            if (!IsAsyncMethod(method))
            {
                continue;
            }

            if (HasCancellationTokenParameter(method))
            {
                continue;
            }

            invalidMethods.Add(method.Identifier.Text);
        }

        return invalidMethods.ToArray();
    }

    private bool IsAsyncMethod(MethodDeclarationSyntax method)
    {
        return method.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword));
    }

    private bool HasCancellationTokenParameter(MethodDeclarationSyntax method)
    {
        return method.ParameterList.Parameters.Any(p =>
            p.Type != null &&
            (p.Type.ToString() == "CancellationToken" ||
             p.Type.ToString() == "System.Threading.CancellationToken"));
    }
}