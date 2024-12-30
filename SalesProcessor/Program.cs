using Asp.Versioning;
using SalesProcessor.Application.Services;
using SalesProcessor.Domain.Interfaces;
using SalesProcessor.Infrastructure.FileParsing;

namespace SalesProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true; 
        });
        
        builder.Services.AddScoped<ICsvSalesParser, CsvSalesParser>();
        builder.Services.AddScoped<ISalesProcessingService, SalesProcessingService>();

        var app = builder.Build();

        app.UseMiddleware<Infrastructure.Middlewares.GlobalExceptionMiddleware>();
        
        //Removing  if (app.Environment.IsDevelopment()) code for
        // interviewers to see the swagger documentation
        app.UseSwagger();
        app.UseSwaggerUI();
        

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}