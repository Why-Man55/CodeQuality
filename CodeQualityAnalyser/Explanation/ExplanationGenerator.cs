using CodeQualityAnalyser.Models;

namespace CodeQualityAnalyser.Explanation
{
    public class ExplanationGenerator : IExplanationGenerator
    {
        private readonly ExplanationTemplates _templates;

        public ExplanationGenerator()
        {
            _templates = new ExplanationTemplates();
        }

        public Models.Explanation Generate(AnalysisResult result)
        {
            if (result == null)
            {
                return new Models.Explanation
                {
                    Title = "Ошибка",
                    PlainExplanation = "Передан пустой результат анализа"
                };
            }

            switch (result.DiagnosticId)
            {
                case "ASYNC001":
                    return _templates.AsyncVoidExplanation(result);
                case "ASYNC002":
                    return _templates.TaskResultExplanation(result);
                case "ASYNC003":
                    return _templates.MissingCancellationTokenExplanation(result);
                case "COMPLEX001":
                    return _templates.ComplexityExplanation(result);
                case "METHOD001":
                    return _templates.MethodSizeExplanation(result);
                case "PARAM001":
                    return _templates.MethodParametersExplanation(result);
                case "CATCH001":
                    return _templates.EmptyCatchExplanation(result);
                default:
                    return _templates.GenericExplanation(result);
            }
        }
    }
}