using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser;

public class EmptyCatchAnalyser : IAnalyser
{
    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var root = tree.GetRoot();

        foreach (var catchClause in root.DescendantNodes().OfType<CatchClauseSyntax>())
        {
            if (!catchClause.Block.Statements.Any())
            {
                errors.Add("[AntiPattern] Обнаружен пустой блок catch! Нельзя молча подавлять ошибки. Добавь логирование или 'throw;'.");
            }
        }

        return errors;
    }
}
