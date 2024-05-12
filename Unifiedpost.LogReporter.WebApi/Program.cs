using Unifiedpost.LogReporter.BusinessLogic.Contracts;
using Unifiedpost.LogReporter.BusinessLogic.Implementations;
using Unifiedpost.LogReporter.WebApi.Configurations;
using Unifiedpost.LogReporter.WebApi.Contracts;
using Unifiedpost.LogReporter.WebApi.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKeyedScoped<IFileHandler, CsvHandler>("csv");
builder.Services.AddKeyedScoped<ILogReporterService, CsvLogReporterService>("csv");
builder.Services.AddScoped<ILogProcessor, LogProcessor>();
builder.Services.AddOptions<CsvConfiguration>().Bind(builder.Configuration.GetSection("CsvConfiguration")).ValidateOnStart();
builder.Services.AddOptions<LogFileConfiguration>().Bind(builder.Configuration.GetSection("LogConfiguration")).ValidateOnStart();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
