using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class AsyncVoidAnalyser : IAnalyser
{
    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();

        foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            var isAsync = method.Modifiers.Any(SyntaxKind.AsyncKeyword);
            var isVoid = method.ReturnType is PredefinedTypeSyntax predefinedType &&
                         predefinedType.Keyword.IsKind(SyntaxKind.VoidKeyword);

            if (isAsync && isVoid)
            {
                errors.Add($"[AsyncError] {fileName}: Метод '{method.Identifier.Text}' объявлен как async void! Асинхронные методы должны возвращать Task или Task<T>, иначе упадет все приложение.");
            }
        }

        return errors;
    }
}
