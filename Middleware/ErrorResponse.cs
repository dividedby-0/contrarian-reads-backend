namespace contrarian_reads_backend.Middleware;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string TraceId { get; set; }
    public string DeveloperMessage { get; set; }
}