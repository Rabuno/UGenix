using UGenix.Shared.Abstractions;

namespace UGenix.API.Models;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public Error? Error { get; init; }
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) 
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> FailureResponse(Error error, string? message = null) 
        => new() { Success = false, Error = error, Message = message };
}

public class CustomProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public IEnumerable<Error>? Errors { get; set; }
}

