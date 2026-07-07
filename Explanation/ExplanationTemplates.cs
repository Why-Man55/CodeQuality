using CodeQualityCoach.Core.Models;

namespace CodeQualityCoach.Core.Explanation
{
    public class ExplanationTemplates
    {
        public Models.Explanation AsyncVoidExplanation(AnalysisResult result)
        {
            var explanation = new Models.Explanation();
            explanation.DiagnosticId = result.DiagnosticId;
            explanation.Title = "⚠️ Использование async void";
            explanation.PlainExplanation = "Метод `" + result.CodeSnippet + "` использует async void. " +
                "Это плохая практика, потому что исключения не будут перехвачены вызывающим кодом.";
            explanation.Recommendation = "Измените возвращаемый тип с `void` на `Task` или `Task<T>`.";
            explanation.ExampleFixedCode =
                "public async Task LoadDataAsync()\n" +
                "{\n" +
                "    await Task.Delay(100);\n" +
                "}";
            explanation.Url = "https://docs.microsoft.com/ru-ru/dotnet/csharp/async";
            return explanation;
        }

        public Models.Explanation TaskResultExplanation(AnalysisResult result)
        {
            var explanation = new Models.Explanation();
            explanation.DiagnosticId = result.DiagnosticId;
            explanation.Title = "⛔ Использование .Result или .Wait()";
            explanation.PlainExplanation = "Код использует `" + result.CodeSnippet + "`. " +
                "Это синхронно блокирует поток и может вызвать дедлок.";
            explanation.Recommendation = "Замените `.Result` на `await`.";
            explanation.ExampleFixedCode = "var data = await GetDataAsync();";
            explanation.Url = "https://docs.microsoft.com/ru-ru/dotnet/csharp/asynchronous-programming/async-best-practices";
            return explanation;
        }

        public Models.Explanation MissingCancellationTokenExplanation(AnalysisResult result)
        {
            var explanation = new Models.Explanation();
            explanation.DiagnosticId = result.DiagnosticId;
            explanation.Title = "🧵 Метод не поддерживает отмену";
            explanation.PlainExplanation = "Метод `" + result.CodeSnippet + "` не принимает CancellationToken.";
            explanation.Recommendation = "Добавьте параметр `CancellationToken cancellationToken = default`.";
            explanation.ExampleFixedCode =
                "public async Task LoadDataAsync(CancellationToken cancellationToken = default)\n" +
                "{\n" +
                "    await httpClient.GetAsync(url, cancellationToken);\n" +
                "}";
            explanation.Url = "https://docs.microsoft.com/ru-ru/dotnet/standard/parallel-programming/task-cancellation";
            return explanation;
        }

        public Models.Explanation ComplexityExplanation(AnalysisResult result)
        {
            var explanation = new Models.Explanation();
            explanation.DiagnosticId = result.DiagnosticId;
            explanation.Title = "🧩 Высокая цикломатическая сложность";
            explanation.PlainExplanation = "Метод имеет высокую сложность, что делает код трудным для понимания.";
            explanation.Recommendation = "Разбейте метод на несколько меньших методов.";
            explanation.ExampleFixedCode = "// Разделите логику на несколько методов";
            explanation.Url = "https://docs.microsoft.com/ru-ru/dotnet/fundamentals/code-analysis/quality-rules/ca1502";
            return explanation;
        }

        public Models.Explanation GenericExplanation(AnalysisResult result)
        {
            var explanation = new Models.Explanation();
            explanation.DiagnosticId = result.DiagnosticId;
            explanation.Title = "📌 " + result.Message;
            explanation.PlainExplanation = "Обратите внимание на это предупреждение. " +
                "Оно указывает на потенциальную проблему в коде.";
            explanation.Recommendation = "Проверьте документацию для более детальной информации.";
            explanation.ExampleFixedCode = "// Исправьте код согласно рекомендациям.";
            explanation.Url = "https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/";
            return explanation;
        }
    }
}