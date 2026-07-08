# Code Quality Coach

Учебный проект для анализа качества C# кода.

## Структура

Один проект `CodeQualityAnalyser` (веб-приложение), внутри которого:

```
CodeQualityAnalyser/
├── Roslyn/           — загрузка syntax trees
├── AnalysServices/   — анализаторы (IAnalyser)
├── Practice/         — генерация отчётов (HTML, JSON, TXT)
├── Integration/      — связка анализаторов и маппинг результатов
├── Explanation/      — шаблоны объяснений
├── Endpoints/        — API
├── Services/         — бизнес-логика
└── wwwroot/          — веб-интерфейс
```

## Сборка и запуск

```bash
dotnet build CodeQuality.sln
dotnet run --project CodeQualityAnalyser/CodeQualityAnalyser.csproj
```

Откройте `http://localhost:5221`.

## API

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/health` | Проверка доступности |
| POST | `/api/analyze` | Анализ `.sln` / `.zip`, ответ JSON |
| POST | `/api/analyze/report/{format}` | Анализ + отчёт (`html`, `json`, `txt`) |
