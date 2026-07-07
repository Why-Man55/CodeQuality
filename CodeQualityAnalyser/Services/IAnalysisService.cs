using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Services;

public interface IAnalysisService
{
    Task<AnalysisResultDto> AnalyzeSolutionAsync(IFormFile solutionFile, CancellationToken cancellationToken);
}
