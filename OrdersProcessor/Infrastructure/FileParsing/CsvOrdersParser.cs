using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using OrdersProcessor.Domain.Entities;
using OrdersProcessor.Domain.Exceptions;
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
            var records = csv.GetRecordsAsync<Order>();
            return records;
        }
        catch (HeaderValidationException)
        {
            // Repassa a exceção diretamente
            throw;
        }
        catch (Exception e)
        {
            throw new InvalidFileException("An error occurred while processing the file. " + e.Message);
        }
    }

}