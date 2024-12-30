using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using OrdersProcessor.Domain.Entities;
using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Infrastructure.FileParsing;

public class CsvOrdersParser : ICsvOrdersParser
{
    private readonly CsvConfiguration _csvConfiguration;
    public CsvOrdersParser()
    {
        _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
        };
    }

    public async Task<IAsyncEnumerable<Order>> ParseAsync(IFormFile file)
    {
        try
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            
            var reader = new StreamReader(stream);
            var csv = new CsvReader(reader, _csvConfiguration);

            csv.Context.RegisterClassMap<OrderColumnsMap>();
            return csv.GetRecordsAsync<Order>(); 
        }
        catch (HeaderValidationException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while processing the file." + e.Message);
            throw;
        }
    }
}