namespace CodeQualityAnalyser.Models
{
    public class Explanation
    {
        public string DiagnosticId { get; set; }
        public string Title { get; set; }
        public string PlainExplanation { get; set; }
        public string Recommendation { get; set; }
        public string ExampleFixedCode { get; set; }
        public string Url { get; set; }

        public Explanation()
        {
            DiagnosticId = string.Empty;
            Title = string.Empty;
            PlainExplanation = string.Empty;
            Recommendation = string.Empty;
            ExampleFixedCode = string.Empty;
            Url = string.Empty;
        }
    }
}