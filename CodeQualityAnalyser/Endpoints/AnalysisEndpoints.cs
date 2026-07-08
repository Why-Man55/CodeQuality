using CodeQualityAnalyser.Integration;
using CodeQualityAnalyser.Practice;
using CodeQualityAnalyser.Practice.ReportGenerators;
using CodeQualityAnalyser.Services;

namespace CodeQualityAnalyser.Endpoints;

public static class AnalysisEndpoints
{
    private const long MaxSolutionSizeBytes = 10 * 1024 * 1024;

    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/health", () => Results.Ok(new { status = "ok" }));

        group.MapPost("/analyze", AnalyzeAsync).DisableAntiforgery();
        group.MapPost("/analyze/report/{format}", AnalyzeReportAsync).DisableAntiforgery();

        return app;
    }

    private static async Task<IResult> AnalyzeAsync(
        IFormFile file,
        IAnalysisService analysisService,
        CancellationToken cancellationToken)
    {
        var validationResult = ValidateUpload(file);
        if (validationResult is not null)
        {
            return validationResult;
        }

        var result = await analysisService.AnalyzeSolutionAsync(file, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> AnalyzeReportAsync(
        string format,
        IFormFile file,
        IAnalysisService analysisService,
        CancellationToken cancellationToken)
    {
        var validationResult = ValidateUpload(file);
        if (validationResult is not null)
        {
            return validationResult;
        }

        var generators = new Dictionary<string, IReportGenerator>(StringComparer.OrdinalIgnoreCase)
        {
            ["html"] = new HtmlReportGenerator(),
            ["json"] = new JsonReportGenerator(),
            ["txt"] = new TxtReportGenerator()
        };

        if (!generators.TryGetValue(format, out var generator))
        {
            return Results.BadRequest(new { error = "Поддерживаются форматы: html, json, txt." });
        }

        var result = await analysisService.AnalyzeSolutionAsync(file, cancellationToken);
        var report = ReportMapper.ToReport(result);
        var content = generator.Generate(report);

        return Results.Content(content, GetContentType(format));
    }

    private static IResult? ValidateUpload(IFormFile file)
    {
        if (file.Length == 0)
        {
            return Results.BadRequest(new { error = "Файл не выбран или пуст." });
        }

        if (!file.FileName.EndsWith(".sln", StringComparison.OrdinalIgnoreCase) &&
            !file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest(new { error = "Поддерживаются файлы решений (.sln) и ZIP-архивы (.zip)." });
        }

        if (file.Length > MaxSolutionSizeBytes)
        {
            return Results.BadRequest(new { error = "Размер файла не должен превышать 10 МБ." });
        }

        return null;
    }

    private static string GetContentType(string format) =>
        format.ToLowerInvariant() switch
        {
            "html" => "text/html; charset=utf-8",
            "json" => "application/json; charset=utf-8",
            "txt" => "text/plain; charset=utf-8",
            _ => "application/octet-stream"
        };
}
