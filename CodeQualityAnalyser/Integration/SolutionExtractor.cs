using System.IO.Compression;

namespace CodeQualityAnalyser.Integration;

public static class SolutionExtractor
{
    public static void ExtractZip(string zipPath, string destinationDirectory)
    {
        ZipFile.ExtractToDirectory(zipPath, destinationDirectory, overwriteFiles: true);
    }

    public static string? FindSolutionFile(string rootDirectory)
    {
        return Directory
            .EnumerateFiles(rootDirectory, "*.sln", SearchOption.AllDirectories)
            .FirstOrDefault();
    }

    public static List<string> FindCSharpFiles(string rootDirectory)
    {
        return Directory
            .EnumerateFiles(rootDirectory, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
