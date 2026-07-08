using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Services;

public sealed class MockAnalysisService : IAnalysisService
{
    public Task<AnalysisResultDto> AnalyzeSolutionAsync(IFormFile solutionFile, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var projectName = Path.GetFileNameWithoutExtension(solutionFile.FileName);
        var issues = CreateMockIssues(projectName);

        var result = new AnalysisResultDto
        {
            ProjectName = projectName,
            AnalyzedAt = DateTime.UtcNow,
            Issues = issues,
            TotalIssues = issues.Count
        };

        return Task.FromResult(result);
    }

    private static IReadOnlyList<IssueDto> CreateMockIssues(string projectName)
    {
        return
        [
            new IssueDto
            {
                RuleId = "CQ001",
                Title = "Использование async void",
                Description = "Метод ProcessData объявлен как async void. Такие методы нельзя ожидать через await, а необработанные исключения могут привести к падению приложения.",
                Recommendation = "Замените async void на async Task или async Task<T>, если метод должен возвращать результат.",
                Severity = "Warning",
                FilePath = $"{projectName}/Services/DataProcessor.cs",
                Line = 42,
                Column = 9
            },
            new IssueDto
            {
                RuleId = "CQ002",
                Title = "Синхронное ожидание Task.Result",
                Description = "Вызов Task.Result блокирует текущий поток и может вызвать взаимную блокировку (deadlock), особенно в UI-приложениях.",
                Recommendation = "Используйте await вместо .Result или .Wait().",
                Severity = "Error",
                FilePath = $"{projectName}/Controllers/ReportController.cs",
                Line = 18,
                Column = 27
            },
            new IssueDto
            {
                RuleId = "CQ003",
                Title = "Отсутствует CancellationToken",
                Description = "Асинхронный метод LoadItemsAsync не принимает CancellationToken. Отмена длительной операции становится невозможной.",
                Recommendation = "Добавьте параметр CancellationToken cancellationToken = default и передавайте его во внутренние вызовы.",
                Severity = "Warning",
                FilePath = $"{projectName}/Repositories/ItemRepository.cs",
                Line = 31,
                Column = 5
            }
        ];
    }
}
