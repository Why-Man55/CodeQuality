namespace CodeAnalyzer;

public interface Analyze
{
    string[] startTest(string fileContent);
}

public class MethodSizeAnalyze : Analyze
{
    private const int MaxLineCount = 30;

    public string[] startTest(string fileContent)
    {
        var invalidMethods = new List<string>();
        
        int index = 0;
        int length = fileContent.Length;

        bool inSingleLineComment = false;
        bool inMultiLineComment = false;
        bool inString = false;
        bool inChar = false;

        int bracePositionCount = 0; // Баланс фигурных скобок 
        
        int methodStartLine = 0;
        int currentLine = 1;
        
        string currentMethodName = null;

        while (index < length)
        {
            char ch = fileContent[index];

            // --- 1. Обработка переносов строк ---
            if (ch == '\n')
            {
                currentLine++;
                inSingleLineComment = false; // Однострочный комментарий сбрасывается в конце строки
                index++;
                continue;
            }
            if (ch == '\r')
            {
                // Пропускаем возврат каретки (для Windows-переносов \r\n), чтобы не двоить строки
                index++;
                continue;
            }

            // --- 2. Пропускаем комментарии и строки (чтобы не путать скобки внутри них) ---
            if (inSingleLineComment) { index++; continue; }
            
            if (inMultiLineComment)
            {
                if (ch == '*' && index + 1 < length && fileContent[index + 1] == '/')
                {
                    inMultiLineComment = false;
                    index += 2;
                }
                else
                {
                    index++;
                }
                continue;
            }

            if (inString)
            {
                if (ch == '\\' && index + 1 < length) { index += 2; continue; } // Пропускаем экранированные символы типа \"
                if (ch == '"') { inString = false; }
                index++;
                continue;
            }

            if (inChar)
            {
                if (ch == '\\' && index + 1 < length) { index += 2; continue; }
                if (ch == '\'') { inChar = false; }
                index++;
                continue;
            }

            // --- 3. Проверка на начало комментариев и строк ---
            if (ch == '/' && index + 1 < length && fileContent[index + 1] == '/')
            {
                inSingleLineComment = true;
                index += 2;
                continue;
            }
            if (ch == '/' && index + 1 < length && fileContent[index + 1] == '*')
            {
                inMultiLineComment = true;
                index += 2;
                continue;
            }
            if (ch == '"') { inString = true; index++; continue; }
            if (ch == '\'') { inChar = true; index++; continue; }

            // --- 4. Логика подсчета скобок и анализа методов ---
            if (ch == '{')
            {
                bracePositionCount++;
                
                // Если это скобка уровня 1 (или 2, в зависимости от наличия namespace/class), 
                // и мы перед этим распарсили имя метода — значит метод НАЧАЛСЯ.
                // Для простоты: если мы заходим в блок, а имя метода уже найдено на уровне класса
                if (bracePositionCount == 2 || bracePositionCount == 3) 
                {
                    // Пытаемся найти имя метода, если мы только что вошли в тело
                    string potentialName = FindMethodNameBefore(fileContent, index);
                    if (!string.IsNullOrEmpty(potentialName))
                    {
                        currentMethodName = potentialName;
                        methodStartLine = currentLine;
                    }
                }
            }
            else if (ch == '}')
            {
                // Метод ЗАКОНЧИЛСЯ
                if (currentMethodName != null && (bracePositionCount == 2 || bracePositionCount == 3))
                {
                    int totalLines = currentLine - methodStartLine - 1;
                    
                    if (totalLines > MaxLineCount)
                    {
                        invalidMethods.Add(currentMethodName);
                    }
                    
                    currentMethodName = null; // Сбрасываем для следующего метода
                }
                
                bracePositionCount--;
            }

            index++;
        }

        return invalidMethods.ToArray();
    }

    // Вспомогательный метод, который отматывает код назад от фигурной скобки
    // и вытягивает имя метода (слово перед круглыми скобками)
    private string FindMethodNameBefore(string code, int openBraceIndex)
    {
        int i = openBraceIndex - 1;
        
        // Шаг А: Ищем закрывающую круглую скобку ')' метода
        while (i > 0 && code[i] != ')')
        {
            if (code[i] == ';' || code[i] == '}') return null; // Уперлись в другое выражение
            i--;
        }
        if (i <= 0) return null;

        // Шаг Б: Ищем открывающую круглую скобку '(' метода, пропуская параметры
        int parenthesisCount = 1;
        i--;
        while (i > 0 && parenthesisCount > 0)
        {
            if (code[i] == ')') parenthesisCount++;
            if (code[i] == '(') parenthesisCount--;
            i--;
        }

        // Шаг В: Пропускаем пробелы между именем метода и скобкой '('
        while (i > 0 && char.IsWhiteSpace(code[i])) i--;

        // Шаг Г: Считываем само имя метода (буквы, цифры, подчёркивание)
        int nameEnd = i + 1;
        while (i >= 0 && (char.IsLetterOrDigit(code[i]) || code[i] == '_')) i--;
        int nameStart = i + 1;

        if (nameEnd > nameStart)
        {
            string name = code.Substring(nameStart, nameEnd - nameStart);
            // Исключаем ключевые слова, которые тоже используют скобки
            if (name == "if" || name == "while" || name == "for" || name == "foreach" || name == "switch")
                return null;
                
            return name;
        }

        return null;
    }
}