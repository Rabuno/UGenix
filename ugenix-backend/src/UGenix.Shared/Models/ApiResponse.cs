using System.Text.Json.Serialization;

namespace UGenix.Shared.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<ApiError>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> CreateSuccess(T data, string? message = null) 
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> CreateFailure(List<ApiError> errors, string? message = null) 
        => new() { Success = false, Errors = errors, Message = message };
}

public class ApiError
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

