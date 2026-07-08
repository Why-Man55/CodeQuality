using System;
using System.IO;
using System.Threading.Tasks;

namespace Practice
{
    public class FileReportWriter {
        public async Task SaveReportAsync(string content, string filePath) {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Содержимое отчета не может быть пустым", nameof(content));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));
            await File.WriteAllTextAsync(filePath, content);
            Console.WriteLine($"Сохранено: {filePath}");
        }
    }
}