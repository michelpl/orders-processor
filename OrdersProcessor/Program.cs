using Asp.Versioning;
using OrdersProcessor.Application.Helpers;
using OrdersProcessor.Application.Services;
using OrdersProcessor.Domain.Interfaces;
using OrdersProcessor.Infrastructure.FileParsing;

namespace OrdersProcessor;

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
        
        builder.Services.AddScoped<ICsvOrdersParser, CsvOrdersParser>();
        builder.Services.AddScoped<IOrdersProcessingService, OrdersProcessingService>();
        builder.Services.AddScoped<IMedianCalculator, MedianCalculator>();
        builder.Services.AddScoped<IRegionAnalyzer, RegionAnalyzer>();
        builder.Services.AddScoped<IOrdersProcessingService, OrdersProcessingService>();

        var app = builder.Build();

        app.UseMiddleware<Infrastructure.Middlewares.GlobalExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}