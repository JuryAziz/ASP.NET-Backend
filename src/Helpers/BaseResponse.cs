using System.Text.Json.Serialization;

namespace Store.Helpers;
public class BaseResponse<T> where T : class
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    public BaseResponse(bool success, string? msg = null)
    {
        Success = success;
        Message = msg;
    }
    public BaseResponse(T? data = null, bool success = true, string? msg = null)
    {
        Data = data;
        Success = success;
        Message = msg;
    }
}


public class BaseResponseList<T> where T : class
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<T>? Data { get; set; }



    public BaseResponseList(bool success, string? msg = null)
    {
        Success = success;
        Message = msg;
    }
    public BaseResponseList(IEnumerable<T>? data = null, bool success = true, string? msg = null)
    {
        Data = data;
        Success = success;
        Message = msg;
    }

}