namespace GymBookingSystem.Application.Common;

public enum ResultStatus { Success, NotFound, Conflict, BadRequest }

public class ServiceResult
{
    public ResultStatus Status { get; init; }
    public string? Error { get; init; }
    public bool IsSuccess => Status == ResultStatus.Success;

    public static ServiceResult Success() => new() { Status = ResultStatus.Success };
    public static ServiceResult NotFound(string error) => new() { Status = ResultStatus.NotFound, Error = error };
    public static ServiceResult Conflict(string error) => new() { Status = ResultStatus.Conflict, Error = error };
    public static ServiceResult BadRequest(string error) => new() { Status = ResultStatus.BadRequest, Error = error };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; init; }

    public static ServiceResult<T> Success(T value) => new() { Status = ResultStatus.Success, Value = value };
    public static new ServiceResult<T> NotFound(string error) => new() { Status = ResultStatus.NotFound, Error = error };
    public static new ServiceResult<T> Conflict(string error) => new() { Status = ResultStatus.Conflict, Error = error };
    public static new ServiceResult<T> BadRequest(string error) => new() { Status = ResultStatus.BadRequest, Error = error };
}