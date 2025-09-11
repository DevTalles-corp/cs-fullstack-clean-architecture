using System;

namespace TechNotes.Domain.Abtractions;

public class Result
{
  public bool IsSuccessful { get; }
  public bool HasFailed => !IsSuccessful;
  public string? ErrorMessage { get; }

  public Result(bool isSuccessful, string? errorMessage = null)
  {
    IsSuccessful = isSuccessful;
    ErrorMessage = errorMessage;
  }

  public static Result Success() => new(true);
  public static Result Failure(string errorMessage) => new(false, errorMessage);

  public static Result<T> Ok<T>(T? value) => new(value, true, null);
  public static Result<T> Fail<T>(string errorMessage) => new(default, false, errorMessage);
}
public class Result<T> : Result
{
  public T? Value { get; }
  protected internal Result(T? value, bool isSuccessful, string? errorMessage) : base(isSuccessful, errorMessage)
  {
    Value = value;
  }
}
