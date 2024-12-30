using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SalesProcessor.Application.Common;
using SalesProcessor.Domain.Interfaces;
using SalesProcessor.Domain.Exceptions;
using SalesProcessor.Infrastructure.FileParsing;

namespace SalesProcessor.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/sales")]
    public class SalesController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions  = new() { WriteIndented = true };
        private readonly ISalesProcessingService _salesService;
        private readonly ICsvSalesParser _csvSalesParser;
        
        public SalesController(ISalesProcessingService salesService,
            ICsvSalesParser csvSalesParser)
        {
            _salesService = salesService;
            _csvSalesParser = csvSalesParser;
        }

        [HttpPost("upload-csv")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UploadCsvFile(IFormFile? file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new InvalidFileException("'file' field is empty. You must provide a file to process");
                
                var salesList = await _csvSalesParser.ParseAsync(file);
                var result = await _salesService.ProcessSalesSummary(salesList);
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                var errorResponse = new ErrorResponse("An error occurred while sales processing", [ex.Message]);
                return BadRequest(JsonSerializer.Serialize(errorResponse, _jsonSerializerOptions));
            }
        }
        
    }
}