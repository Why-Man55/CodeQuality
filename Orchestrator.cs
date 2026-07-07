public abstract class Analyz
{
    public abstract bool Analyze(string text);
}

public class Orchestrator
{
    private static readonly Analyz[] _tests = new Analyz[10];

    public static bool[] StartTest(string text)
    {
        var results = new bool[_tests.Length];
        Parallel.For(0, _tests.Length, i =>
        {
            try
            {
                results[i] = _tests[i].Analyze(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в тесте {i}: {ex.Message}");
                results[i] = false;
            }
        });

        return results;
    }
    //метод для отсылки результатов на вывод
    private static void sendResults(bool[] results)
    {
        //TODO
    }
}