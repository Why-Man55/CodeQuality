using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Integration;

internal static class OrchestratorIssueMapper
{
    public static bool TryMap(int testIndex, string value, string fileName, out IssueDto issue)
    {
        if (value.Contains('['))
        {
            issue = null!;
            return false;
        }

        issue = testIndex switch
        {
            0 => CreateIssue(
                ruleId: "METHOD001",
                title: "Слишком большой метод",
                description: $"Метод '{value}' содержит более 30 строк кода.",
                recommendation: "Разбейте метод на несколько меньших методов с чёткой ответственностью.",
                fileName: fileName),
            1 => CreateIssue(
                ruleId: "PARAM001",
                title: "Слишком много параметров",
                description: $"Метод '{value}' принимает более 4 параметров.",
                recommendation: "Сократите список параметров или сгруппируйте их в отдельный класс/record.",
                fileName: fileName),
            2 => CreateIssue(
                ruleId: "ASYNC003",
                title: "Отсутствует CancellationToken",
                description: $"Асинхронный метод '{value}' не принимает CancellationToken.",
                recommendation: "Добавьте параметр CancellationToken cancellationToken = default и передавайте его дальше.",
                fileName: fileName),
            _ => null!
        };

        return issue != null;
    }

    private static IssueDto CreateIssue(
        string ruleId,
        string title,
        string description,
        string recommendation,
        string fileName)
    {
        return new IssueDto
        {
            RuleId = ruleId,
            Title = title,
            Description = description,
            Recommendation = recommendation,
            Severity = "Warning",
            FilePath = fileName,
            Line = 0,
            Column = 0
        };
    }
}
