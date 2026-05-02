namespace Application.Common.Responses;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();

    public static BaseResponse<T> SuccessResponse(T data, string message = "Operation successful")
    {
        return new BaseResponse<T> { Data = data, Success = true, Message = message };
    }

    public static BaseResponse<T> FailureResponse(string message, List<string>? errors = null)
    {
        return new BaseResponse<T> { Success = false, Message = message, Errors = errors ?? new List<string>() };
    }
}
