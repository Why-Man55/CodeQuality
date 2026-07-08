using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeQualityAnalyser;

public class TaskResultAnalyzer : Analyze
{
    public string[] startTest(string fileContent)
    {
        var issues = new List<string>();
        SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContent);
        SyntaxNode root = tree.GetRoot();

        // Ищем все обращения к .Result и .Wait()
        var suspiciousNodes = root.DescendantNodes()
            .Where(node =>
            {
                if (node is MemberAccessExpressionSyntax memberAccess)
                {
                    string memberName = memberAccess.Name.Identifier.Text;
                    return memberName == "Result" || memberName == "Wait";
                }
                return false;
            })
            .ToList();

        foreach (var node in suspiciousNodes)
        {
            // Получаем строку с кодом
            string codeSnippet = node.ToString();

            // Получаем номер строки
            var lineSpan = node.GetLocation().GetLineSpan();
            int lineNumber = lineSpan.StartLinePosition.Line + 1;

            // Формируем сообщение об ошибке
            string issue = $"[TASK001] Синхронное ожидание Task: {codeSnippet} | " +
                           $"Строка: {lineNumber} | " +
                           $"Описание: Использование .Result или .Wait() может привести к deadlock'у. | " +
                           $"Исправление: Используйте await вместо .Result/.Wait()";

            issues.Add(issue);
        }
        return issues.ToArray();
    }
}