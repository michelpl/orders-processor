using System.Text;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using OrdersProcessor.Domain.Exceptions;
using OrdersProcessor.Infrastructure.FileParsing;

namespace OrdersProcessor.UnitTests.Infrastructure.FileParsing;

public class CsvOrdersParserTests
{
    private readonly CsvOrdersParser _parser;

    public CsvOrdersParserTests()
    {
        _parser = new CsvOrdersParser();
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnOrders_WhenCsvIsValid()
    {
        // Arrange
        var csvContent = "Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit\n" +
                         "Europe,Denmark,Clothes,Online,C,2/20/2013,473105037,2/28/2013,1149,109.28,35.84,125562.72,41180.16,84382.56\n" +
                         "Europe,Germany,Cosmetics,Offline,M,3/31/2013,754046475,5/3/2013,7964,437.20,263.33,3481860.80,2097160.12,1384700.68";
        
        var file = CreateMockFormFile(csvContent);

        // Act
        var orders = await _parser.ParseAsync(file);

        // Assert
        var orderList = await orders.ToListAsync();
        Assert.NotNull(orderList);
        Assert.Equal(2, orderList.Count);
        Assert.Equal("Europe", orderList[0].Region);
        Assert.Equal("Denmark", orderList[0].Country);
        Assert.Equal("Clothes", orderList[0].ItemType);
    }
    [Fact]
    public async Task ParseAsync_ShouldReturnEmpty_WhenCsvIsEmpty()
    {
        // Arrange
        var csvContent = string.Empty;
        var file = CreateMockFormFile(csvContent);

        // Act
        var orders = await _parser.ParseAsync(file);

        // Assert
        var orderList = await orders.ToListAsync();
        Assert.Empty(orderList);
    }

    [Fact]
    public async Task ParseAsync_ShouldThrowException_WhenFileStreamIsCorrupted()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Stream corrupted"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidFileException>(() => _parser.ParseAsync(mockFile.Object));
    }
    
    private IFormFile CreateMockFormFile(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        var file = new FormFile(stream, 0, bytes.Length, "file", "test.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
        return file;
    }
}