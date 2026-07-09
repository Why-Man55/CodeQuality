using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeQualityCoach.Roslyn;

public class RoslynEngine
{
    /// Метод принимает список путей к файлам 
    
    public async Task<List<SyntaxTree>> GetSyntaxTreesAsync(List<string> filePaths)
    {
        var syntaxTrees = new List<SyntaxTree>();

        foreach (var path in filePaths)
        {
            if (File.Exists(path))
            {
                string code = await File.ReadAllTextAsync(path);
                
                // Превращаем текст в синтаксическое дерево Roslyn
                var tree = CSharpSyntaxTree.ParseText(code, path: path);
                
                
                syntaxTrees.Add(tree);
            }
        }
        
        return syntaxTrees;
    }
}
