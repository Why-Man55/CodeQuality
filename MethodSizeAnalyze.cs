using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer;

public class MethodSizeAnalyze : Analyze
{
    private const int MaxLineCount = 30;

    public string[] startTest(SyntaxNode root)
    {
        var invalidMethods = new List<string>();

        // Поиск всех объявлений методов в переданном корневом узле дерева
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
                // Добавление имени метода в список нарушителей
                invalidMethods.Add(method.Identifier.Text);
            }
        }

        return invalidMethods.ToArray();
    }
}