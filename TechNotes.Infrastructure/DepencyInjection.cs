using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    return services;
  }
}
