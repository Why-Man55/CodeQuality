using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser.AnalysServices;

// возвращаем список ожибок (например номера строк)
public interface IAnalyser
{
    List<string> GetAnalysis(SyntaxTree tree);
}
