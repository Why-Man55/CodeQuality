using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeQualityAnalyser.Roslyn;

public class RoslynEngine
{
    public async Task<List<SyntaxTree>> GetSyntaxTreesAsync(List<string> filePaths)
    {
        var syntaxTrees = new List<SyntaxTree>();

        foreach (var path in filePaths)
        {
            if (File.Exists(path))
            {
                string code = await File.ReadAllTextAsync(path);
                var tree = CSharpSyntaxTree.ParseText(code, path: path);
                syntaxTrees.Add(tree);
            }
        }

        return syntaxTrees;
    }
}
