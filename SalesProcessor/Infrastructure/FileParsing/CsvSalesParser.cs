using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SalesProcessor.Domain.Entities;
using SalesProcessor.Domain.Interfaces;

namespace SalesProcessor.Infrastructure.FileParsing;

public class CsvSalesParser : ICsvSalesParser
{
    private readonly CsvConfiguration _csvConfiguration;
    public CsvSalesParser()
    {
        _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
        };
    }

    public async Task<IAsyncEnumerable<Sale>> ParseAsync(IFormFile file)
    {
        try
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            
            var reader = new StreamReader(stream);
            var csv = new CsvReader(reader, _csvConfiguration);

            csv.Context.RegisterClassMap<SaleColumnsMap>();
            return csv.GetRecordsAsync<Sale>(); 
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