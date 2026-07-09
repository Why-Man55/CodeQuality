using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser.AnalysServices;

public class TaskResultAnalyzer : IAnalyser
{
    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var issues = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();

        var suspiciousNodes = root.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Where(memberAccess =>
            {
                var memberName = memberAccess.Name.Identifier.Text;
                return memberName is "Result" or "Wait";
            });

        foreach (var node in suspiciousNodes)
        {
            var lineNumber = node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            issues.Add(
                $"[TASK001] {fileName}: Synchronous Task wait: {node}. " +
                $"Line: {lineNumber}. Description: Using .Result or .Wait() can cause a deadlock. " +
                "Fix: Use await instead of .Result/.Wait().");
        }

        return issues;
    }
}
