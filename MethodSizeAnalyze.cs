using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer;

public interface Analyze
{
    string[] startTest(string fileContent);
}

public class MethodSizeAnalyze : Analyze
{
    // Максимально допустимое количество строк
    private const int MaxLineCount = 30;

    public string[] startTest(string fileContent)
    {
        var invalidMethods = new List<string>();

        // Парсинг в синтаксическое дерево Roslyn
        SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContent);
        SyntaxNode root = tree.GetRoot();

        // Поиск всех объявлений методов в дереве
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            // Пропуск методов без тела
            if (method.Body == null)
            {
                continue;
            }

            // Получение позиций строк начала и конца тела метода
            var lineSpan = method.Body.GetLocation().GetLineSpan();
            int startLine = lineSpan.StartLinePosition.Line;
            int endLine = lineSpan.EndLinePosition.Line;

            // Расчет количества строк внутри фигурных скобок
            int methodLines = endLine - startLine - 1;

            // Проверка превышения лимита
            if (methodLines > MaxLineCount)
            {
                invalidMethods.Add(method.Identifier.Text);
            }
        }
        
        return invalidMethods.ToArray();
    }
}