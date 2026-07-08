using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Explanation
{
    public interface IExplanationGenerator
    {
        Models.Explanation Generate(AnalysisResult result);
    }
}