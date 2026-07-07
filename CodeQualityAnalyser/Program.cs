using CodeQualityAnalyser.Endpoints;
using CodeQualityAnalyser.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAnalysisService, MockAnalysisService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapAnalysisEndpoints();

app.Run();
