using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

namespace CodeQualityAnalyser;

public class ComplexityAnalyser : IAnalyser
{
    private const int MaxComplexity = 5;

    public List<string> GetAnalysis(SyntaxTree tree)
    {
        var errors = new List<string>();
        var fileName = Path.GetFileName(tree.FilePath);
        var root = tree.GetRoot();
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            var walker = new ComplexityWalker();
            walker.Visit(method);

            if (walker.Score > MaxComplexity)
            {
                errors.Add($"[Complexity] {fileName}: Метод '{method.Identifier.Text}' слишком сложный. Его цикломатическая сложность = {walker.Score} (максимум {MaxComplexity}).");
            }
        }

        return errors;
    }

    private class ComplexityWalker : CSharpSyntaxWalker
    {
        public int Score { get; private set; }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            Score++;
            base.VisitIfStatement(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            Score++;
            base.VisitForStatement(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            Score++;
            base.VisitForEachStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            Score++;
            base.VisitWhileStatement(node);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            Score++;
            base.VisitDoStatement(node);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            Score++;
            base.VisitSwitchStatement(node);
        }

        public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            Score++;
            base.VisitCaseSwitchLabel(node);
        }

        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            Score++;
            base.VisitConditionalExpression(node);
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (node.IsKind(SyntaxKind.LogicalAndExpression) ||
                node.IsKind(SyntaxKind.LogicalOrExpression))
            {
                Score++;
            }

            base.VisitBinaryExpression(node);
        }
        /*
        public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
        }

        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
        }
        */
    }
}
