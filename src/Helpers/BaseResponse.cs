using System.Text.Json.Serialization;

public class BaseResponse<T>(bool success, T? data, string? msg = null)
{
    public required bool Success { get; set; } = success;

    [JsonIgnore]
    public string? Message { get; set; } = msg;

    [JsonIgnore]
    public T? Data { get; set; } = data;
}