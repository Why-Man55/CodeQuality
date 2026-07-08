namespace CodeQualityAnalyser.Integration;

internal static class RuleIdNormalizer
{
    public static string Normalize(string ruleId) =>
        ruleId switch
        {
            "AsyncError" => "ASYNC001",
            "TASK001" => "ASYNC002",
            "Complexity" => "COMPLEX001",
            "AntiPattern" => "CATCH001",
            _ => ruleId
        };
}
