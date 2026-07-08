using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer;

public class MethodParametersAnalyze : Analyze
{
    // Максимально допустимое количество параметров
    private const int MaxParameterCount = 4;

    public string[] startTest(string fileContent)
    {
        var invalidMethods = new List<string>();

        // Парсинг в синтаксическое дерево Roslyn
        SyntaxTree tree = CSharpSyntaxTree.ParseText(fileContent);
        SyntaxNode root = tree.GetRoot();

        // Поиск всех объявлений методов
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            // Получение списка параметров метода
            var parameterList = method.ParameterList;

            // Пропуск проверки, если у метода отсутствует список параметров
            if (parameterList == null)
            {
                continue;
            }

            // Получение точного количества параметров
            int actualParameterCount = parameterList.Parameters.Count;

            // Проверка превышения лимита
            if (actualParameterCount > MaxParameterCount)
            {
                // Добавление имени метода в список нарушителей
                invalidMethods.Add(method.Identifier.Text);
            }
        }

        // Возврат массива строк согласно интерфейсу
        return invalidMethods.ToArray();
    }
}