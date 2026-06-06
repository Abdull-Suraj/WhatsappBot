namespace BubbleShop.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; } = Array.Empty<string>();

    private Result() { }

    public static Result<T> Success(T value) =>
        new() { IsSuccess = true, Value = value };

    public static Result<T> Failure(string error) =>
        new() { IsSuccess = false, Error = error, Errors = new[] { error } };

    public static Result<T> Failure(IReadOnlyList<string> errors) =>
        new() { IsSuccess = false, Error = errors.FirstOrDefault(), Errors = errors };

    public bool IsFailure => !IsSuccess;
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; } = Array.Empty<string>();

    private Result() { }

    public static Result Success() => new() { IsSuccess = true };

    public static Result Failure(string error) =>
        new() { IsSuccess = false, Error = error, Errors = new[] { error } };

    public static Result Failure(IReadOnlyList<string> errors) =>
        new() { IsSuccess = false, Error = errors.FirstOrDefault(), Errors = errors };

    public bool IsFailure => !IsSuccess;
}
