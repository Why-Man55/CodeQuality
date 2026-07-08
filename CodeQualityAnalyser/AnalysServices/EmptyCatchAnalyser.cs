using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class EmptyCatchAnalyser : IAnalyser
{
    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();

        foreach (var catchClause in root.DescendantNodes().OfType<CatchClauseSyntax>())
        {
            if (!catchClause.Block.Statements.Any())
            {
                errors.Add($"[AntiPattern] {fileName}: Обнаружен пустой блок catch! Нельзя молча подавлять ошибки. Добавь логирование или 'throw;'.");
            }
        }

        return errors;
    }
}
