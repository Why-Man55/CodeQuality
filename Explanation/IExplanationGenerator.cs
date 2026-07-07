using CodeQualityCoach.Core.Models;

namespace CodeQualityCoach.Core.Explanation
{
    public interface IExplanationGenerator
    {
        Models.Explanation Generate(AnalysisResult result);
    }
}