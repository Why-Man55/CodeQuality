using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeQualityCoach.Roslyn;

public class RoslynEngine
{
    public async Task<List<string>> AnalyzeFilesAsync(List<string> filePaths)
    {
        var errorsList = new List<string>();
        var syntaxTrees = new List<SyntaxTree>();

        //Читаем все переданные файлы студента
        foreach (var path in filePaths)
        {
            if (File.Exists(path))
            {
                string code = await File.ReadAllTextAsync(path);
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(code));
            }
        }

        //Создаем компиляцию в памяти 
        var compilation = CSharpCompilation.Create("StudentProjectAnalysis")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        //Перебираем файлы для анализа семантики
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = await syntaxTree.GetRootAsync();
        }

        return errorsList;
    }
}