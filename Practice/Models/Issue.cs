namespace Practice.Models {
    public class Issue {
        public string FilePath { get; set; }
        public int Line { get; set; }
        public string Severity { get; set; } 
        public string RuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Suggestion { get; set; }
        public string CodeSnippet { get; set; }
    }
}