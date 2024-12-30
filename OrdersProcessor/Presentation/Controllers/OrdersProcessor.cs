using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrdersProcessor.Application.Common;
using OrdersProcessor.Domain.Exceptions;
using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersProcessor(
        IOrdersProcessingService ordersService,
        ICsvOrdersParser csvOrdersParser)
        : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions  = new() { WriteIndented = true };

        [HttpPost("upload-csv")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UploadCsvFile(IFormFile? file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new InvalidFileException("'file' field is empty. You must provide a file to process");
                
                var ordersList = await csvOrdersParser.ParseAsync(file);
                var result = await ordersService.GetOrdersSummary(ordersList);
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                var errorResponse = new ErrorResponse("An error occurred while the upload process", [ex.Message]);
                return BadRequest(JsonSerializer.Serialize(errorResponse, _jsonSerializerOptions));
            }
        }
        
    }
}