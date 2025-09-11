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
}
