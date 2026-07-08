using Microsoft.CodeAnalysis;

namespace CodeQualityAnalyser;

// возвращаем список ожибок (например номера строк)
public interface IAnalyser
{
    List<string> GetAnalysis(SyntaxTree tree);
}
