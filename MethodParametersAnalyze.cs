using System;
using System.Collections.Generic;

namespace CodeAnalyzer;

public interface Analyze
{
    string[] startTest(string fileContent);
}

public class MethodParametersAnalyze : Analyze
{
    private const int MaxParametersCount = 4;

    public string[] startTest(string fileContent)
    {
        var invalidMethods = new List<string>();
        
        int index = 0;
        int length = fileContent.Length;

        bool inSingleLineComment = false;
        bool inMultiLineComment = false;
        bool inString = false;
        bool inChar = false;

        int bracePositionCount = 0; // Баланс фигурных скобок { }

        while (index < length)
        {
            char ch = fileContent[index];

            // --- 1. Обработка переносов строк ---
            if (ch == '\n') { inSingleLineComment = false; index++; continue; }
            if (ch == '\r') { index++; continue; }

            // --- 2. Пропускаем комментарии и строки ---
            if (inSingleLineComment) { index++; continue; }
            if (inMultiLineComment)
            {
                if (ch == '*' && index + 1 < length && fileContent[index + 1] == '/')
                {
                    inMultiLineComment = false;
                    index += 2;
                }
                else { index++; }
                continue;
            }
            if (inString)
            {
                if (ch == '\\' && index + 1 < length) { index += 2; continue; }
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
            if (ch == '/' && index + 1 < length && fileContent[index + 1] == '/') { inSingleLineComment = true; index += 2; continue; }
            if (ch == '/' && index + 1 < length && fileContent[index + 1] == '*') { inMultiLineComment = true; index += 2; continue; }
            if (ch == '"') { inString = true; index++; continue; }
            if (ch == '\'') { inChar = true; index++; continue; }

            // --- 4. Логика поиска методов по открывающей скобке ---
            if (ch == '{')
            {
                bracePositionCount++;
                
                // Проверяем методы на уровне класса (обычно уровень вложенности 2 или 3)
                if (bracePositionCount == 2 || bracePositionCount == 3)
                {
                    // Вызываем метод анализа параметров
                    string methodName = AnalyzeMethodParams(fileContent, index, out int paramCount);
                    
                    if (!string.IsNullOrEmpty(methodName) && paramCount > MaxParametersCount)
                    {
                        invalidMethods.Add(methodName);
                    }
                }
            }
            else if (ch == '}')
            {
                bracePositionCount--;
            }

            index++;
        }

        return invalidMethods.ToArray();
    }

    // Метод "отматывает" код назад, считает запятые внутри круглых скобок и вытаскивает имя
    private string AnalyzeMethodParams(string code, int openBraceIndex, out int paramCount)
    {
        paramCount = 0;
        int i = openBraceIndex - 1;
        
        // Шаг А: Ищем закрывающую круглую скобку ')' метода
        while (i > 0 && code[i] != ')')
        {
            if (code[i] == ';' || code[i] == '}') return null; 
            i--;
        }
        if (i <= 0) return null;

        int closeParenthesisIndex = i;

        // Шаг Б: Идем назад до открывающей скобки '(' и считаем параметры
        int parenthesisCount = 1;
        int commaCount = 0;
        bool hasContent = false;
        i--;
        
        while (i > 0 && parenthesisCount > 0)
        {
            char current = code[i];
            
            if (current == ')') parenthesisCount++;
            if (current == '(') 
            {
                parenthesisCount--;
                if (parenthesisCount == 0) break;
            }

            // Если мы внутри скобок самого метода (parenthesisCount == 1)
            if (parenthesisCount == 1)
            {
                if (current == ',') commaCount++;
                if (!char.IsWhiteSpace(current) && current != ',') hasContent = true;
            }
            
            i--;
        }

        // Вычисляем количество параметров
        if (hasContent)
        {
            paramCount = commaCount + 1;
        }

        // Шаг В: Пропускаем пробелы перед именем метода
        while (i > 0 && char.IsWhiteSpace(code[i])) i--;

        // Шаг Г: Считываем имя метода
        int nameEnd = i + 1;
        while (i >= 0 && (char.IsLetterOrDigit(code[i]) || code[i] == '_')) i--;
        int nameStart = i + 1;

        if (nameEnd > nameStart)
        {
            string name = code.Substring(nameStart, nameEnd - nameStart);
            
            // Защита от ключевых слов
            if (name == "if" || name == "while" || name == "for" || name == "foreach" || name == "switch")
                return null;
                
            return name;
        }

        return null;
    }
}