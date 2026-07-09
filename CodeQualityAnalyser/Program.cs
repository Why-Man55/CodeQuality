using CodeQualityAnalyser.Roslyn;
using CodeQualityAnalyser.Endpoints;
using CodeQualityAnalyser.Explanation;
using CodeQualityAnalyser.Integration;
using CodeQualityAnalyser.Practice;
using CodeQualityAnalyser.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RoslynEngine>();
builder.Services.AddSingleton<IExplanationGenerator, ExplanationGenerator>();
builder.Services.AddSingleton<IssueEnricher>();
builder.Services.AddSingleton<AnalyzerRunner>();
builder.Services.AddSingleton<IAnalysisService, AnalysisService>();
builder.Services.AddSingleton<IReportService, ReportService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapAnalysisEndpoints();

app.Run();
