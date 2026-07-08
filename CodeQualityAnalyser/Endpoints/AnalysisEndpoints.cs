using CodeQualityAnalyser.Services;

namespace CodeQualityAnalyser.Endpoints;

public static class AnalysisEndpoints
{
    private const long MaxSolutionSizeBytes = 10 * 1024 * 1024;

    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/health", () => Results.Ok(new { status = "ok" }));

        group.MapPost("/analyze", async (
            IFormFile file,
            IAnalysisService analysisService,
            CancellationToken cancellationToken) =>
        {
            if (file.Length == 0)
            {
                return Results.BadRequest(new { error = "Файл не выбран или пуст." });
            }

            if (!file.FileName.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest(new { error = "Поддерживаются только файлы решений (.sln)." });
            }

            if (file.Length > MaxSolutionSizeBytes)
            {
                return Results.BadRequest(new { error = "Размер файла не должен превышать 10 МБ." });
            }

            var result = await analysisService.AnalyzeSolutionAsync(file, cancellationToken);
            return Results.Ok(result);
        })
        .DisableAntiforgery();

        return app;
    }
}
