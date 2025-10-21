using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using TechNotes;
using TechNotes.Application;
using TechNotes.Application.Notes;
using TechNotes.Components;
using TechNotes.Features.Notes.Services;
using TechNotes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddDataProtection().SetApplicationName("TechNotes");

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => false;
  options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
  options.Cookie.SameSite = SameSiteMode.Lax;
  options.Cookie.Name = "TechNotes.Auth";
});

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ExternalScheme, options =>
{
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
  options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<INoteColorService, NoteColorService>();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
  googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
  googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
  googleOptions.CallbackPath = "/signin-google";
  googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
  googleOptions.SaveTokens = true;

  googleOptions.CorrelationCookie.HttpOnly = true;
  googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
  googleOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;

  googleOptions.Scope.Add("email");
  googleOptions.Scope.Add("profile");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
