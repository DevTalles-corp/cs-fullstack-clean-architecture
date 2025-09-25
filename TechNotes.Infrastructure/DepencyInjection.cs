using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechNotes.Application.Authentication;
using TechNotes.Domain.Notes;
using TechNotes.Infrastructure.Authentication;
using TechNotes.Infrastructure.Repositories;

namespace TechNotes.Infrastructure;

public static class DepencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configutarion)
  {
    services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(
        configutarion.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TechNotes.Infrastructure")
      )
    );
    services.AddScoped<INoteRepository, NoteRepository>();
    AddAuthentication(services);
    return services;
  }
  private static void AddAuthentication(IServiceCollection services)
  {
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
    services.AddCascadingAuthenticationState();
    services.AddAuthorization();
    services.AddAuthentication(options =>
    {
      options.DefaultScheme = IdentityConstants.ApplicationScheme;
      options.DefaultChallengeScheme = IdentityConstants.ExternalScheme;
    }).AddIdentityCookies();
    services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
  }
}
