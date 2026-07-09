using CodeQualityAnalyser.Integration;
using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser;

public class Orchestrator
{
    private static readonly Analyze[] _tests =
    [
        new MethodSizeAnalyze(),
        new MethodParametersAnalyze(),
        new CancellationTokenAnalyzeAdapter(),
        new TaskResultAnalyzerAdapter()
    ];
    
    public static async Task<string[][]> startTest(SyntaxNode text)
    {
        var tasks = new Task<string[]>[_tests.Length];
        for (int i = 0; i < _tests.Length; i++)
        {
            int index = i;
         
            tasks[index] = Task.Run(() =>
            {
                try
                {
                    // Проверка на случай, если ячейка массива тестов пустая
                    if (_tests[index] == null) return Array.Empty<string>();

                    // Передаем готовый root в тест
                    return _tests[index].startTest(text);
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