namespace OrdersProcessor.Application.Common;

public class ErrorResponse  
{
    public string Message { get; set; }
    public List<string> Details { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
        Details = new List<string>();
    }

    public ErrorResponse(string message, List<string> details)
    {
        Message = message;
        Details = details;
    }
}
