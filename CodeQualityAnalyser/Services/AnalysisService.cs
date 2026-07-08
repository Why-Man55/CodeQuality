using CodeQualityAnalyser.Integration;
using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Services;

public sealed class AnalysisService : IAnalysisService
{
    private readonly AnalyzerRunner _analyzerRunner;

    public AnalysisService(AnalyzerRunner analyzerRunner)
    {
        _analyzerRunner = analyzerRunner;
    }

    public async Task<AnalysisResultDto> AnalyzeSolutionAsync(IFormFile solutionFile, CancellationToken cancellationToken)
    {
        var workDirectory = Path.Combine(Path.GetTempPath(), "code-quality-coach", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workDirectory);

        try
        {
            var extension = Path.GetExtension(solutionFile.FileName).ToLowerInvariant();
            string projectName;
            List<string> csharpFiles;

            switch (extension)
            {
                case ".zip":
                {
                    var zipPath = Path.Combine(workDirectory, Path.GetFileName(solutionFile.FileName));
                    await using (var stream = File.Create(zipPath))
                    {
                        await solutionFile.CopyToAsync(stream, cancellationToken);
                    }

                    SolutionExtractor.ExtractZip(zipPath, workDirectory);
                    var solutionPath = SolutionExtractor.FindSolutionFile(workDirectory);
                    projectName = solutionPath != null
                        ? Path.GetFileNameWithoutExtension(solutionPath)
                        : Path.GetFileNameWithoutExtension(solutionFile.FileName);
                    csharpFiles = SolutionExtractor.FindCSharpFiles(workDirectory);
                    break;
                }
                case ".sln":
                {
                    var solutionPath = Path.Combine(workDirectory, Path.GetFileName(solutionFile.FileName));
                    await using (var stream = File.Create(solutionPath))
                    {
                        await solutionFile.CopyToAsync(stream, cancellationToken);
                    }

                    projectName = Path.GetFileNameWithoutExtension(solutionFile.FileName);
                    csharpFiles = SolutionExtractor.FindCSharpFiles(workDirectory);
                    break;
                }
                default:
                    throw new InvalidOperationException("Поддерживаются файлы .sln и .zip.");
            }

            if (csharpFiles.Count == 0)
            {
                return CreateEmptySourcesResult(projectName);
            }

            var issues = await _analyzerRunner.RunAndMapAsync(csharpFiles, cancellationToken);

            return new AnalysisResultDto
            {
                ProjectName = projectName,
                AnalyzedAt = DateTime.UtcNow,
                Issues = issues,
                TotalIssues = issues.Count
            };
        }
        finally
        {
            try
            {
                Directory.Delete(workDirectory, recursive: true);
            }
            catch
            {
                // Временная папка будет удалена ОС позже.
            }
        }
    }

    private static AnalysisResultDto CreateEmptySourcesResult(string projectName)
    {
        return new AnalysisResultDto
        {
            ProjectName = projectName,
            AnalyzedAt = DateTime.UtcNow,
            Issues =
            [
                new IssueDto
                {
                    RuleId = "SYS001",
                    Title = "Исходные файлы не найдены",
                    Description = "В загруженном архиве или рядом с .sln не найдено файлов .cs.",
                    Recommendation = "Загрузите ZIP-архив с файлом .sln и исходным кодом проекта.",
                    Severity = "Info",
                    FilePath = projectName,
                    Line = 0,
                    Column = 0
                }
            ],
            TotalIssues = 1
        };
    }
}
