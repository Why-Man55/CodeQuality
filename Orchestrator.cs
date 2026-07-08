using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeAnalyzer;

public class Orchestrator
{
    private static readonly Analyze[] _tests = new Analyze[2] 
    { 
        
    };

    // Метод стал async и теперь возвращает Task с массивом результатов
    public static async Task<string[][]> startTest(SyntaxNode text)
    {
        var tasks = new Task<string[]>[_tests.Length];

        for (int i = 0; i < _tests.Length; i++)
        {
            int index = i; // Локальная переменная для безопасного замыкания в многопоточности

            // Task.Run запускает тест параллельно в пуле потоков
            tasks[index] = Task.Run(() =>
            {
                try
                {
                    // Проверка на случай, если ячейка массива тестов пустая
                    if (_tests[index] == null) return Array.Empty<string>();

                    // Передаем готовый root в тест
                    return _tests[index].startTest(root);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в тесте {index}: {ex.Message}");
                    // В случае ошибки возвращаем пустой массив строк вместо false
                    return Array.Empty<string>();
                }
            });
        }
        
        string[][] results = await Task.WhenAll(tasks);

        // Отправка результатов
        sendResults(results);

        return results;
    }

    private static void sendResults(string[][] results)
    {
        // TODO
    }
}