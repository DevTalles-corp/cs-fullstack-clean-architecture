using System;
using Microsoft.Extensions.DependencyInjection;
using TechNotes.Application.Notes;

namespace TechNotes.Application;

public static class DependencyInyection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<INoteService, NoteService>();
    return services;
  }
}
