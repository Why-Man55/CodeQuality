using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer;

public class MethodParametersAnalyze : Analyze
{
    private const int MaxParameterCount = 4;

    public string[] startTest(SyntaxNode root)
    {
        var invalidMethods = new List<string>();

        // Поиск всех объявлений методов в переданном корневом узле дерева
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

            // Получение точного количества параметров из синтаксического узла
            int actualParameterCount = parameterList.Parameters.Count;

            // Проверка превышения лимита
            if (actualParameterCount > MaxParameterCount)
            {
                // Добавление имени метода в список нарушителей
                invalidMethods.Add(method.Identifier.Text);
            }
        }

        return invalidMethods.ToArray();
    }
}