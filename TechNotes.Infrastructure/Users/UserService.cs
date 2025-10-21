using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TechNotes.Application.Exceptions;
using TechNotes.Application.Users;
using TechNotes.Domain.Notes;

namespace TechNotes.Infrastructure.Users;

public class UserService : IUserService
{
  private readonly UserManager<User> _userManager;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly INoteRepository _noteRepository;

  private readonly RoleManager<IdentityRole> _roleManager;

  public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
                     INoteRepository noteRepository, RoleManager<IdentityRole> roleManager)
  {
    _userManager = userManager;
    _httpContextAccessor = httpContextAccessor;
    _noteRepository = noteRepository;
    _roleManager = roleManager;
  }

  public async Task AddUserRoleAsync(string userId, string roleName)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
      return;
    }
    if (!await _roleManager.RoleExistsAsync(roleName))
    {
      var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
      if (!roleResult.Succeeded)
      {
        throw new Exception("Error al crear el rol");
      }
    }
    var result = await _userManager.AddToRoleAsync(user, roleName);
    if (!result.Succeeded)
    {
      throw new Exception("Error al agregar el rol al usuario");
    }
  }

  public async Task<bool> CurrentUserCanCreateNoteAsync()
  {
    var user = await GetCurrentUserAsync();
    if (user is null)
    {
      return false;
    }
    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
    var isWriter = await _userManager.IsInRoleAsync(user, "Writer");
    return isAdmin || isWriter;
  }

  public async Task<bool> CurrentUserCanEditNoteAsync(int noteId)
  {
    var user = await GetCurrentUserAsync();
    if (user is null)
    {
      return false;
    }
    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
    var isWriter = await _userManager.IsInRoleAsync(user, "Writer");
    var note = await _noteRepository.GetNoteByIdAsync(noteId);
    if (note is null)
    {
      return false;
    }
    var isAuthorized = isAdmin || (isWriter && note.UserId == user.Id);
    return isAuthorized;
  }

  public async Task<string> GetCurrentUserIdAsync()
  {
    var user = await GetCurrentUserAsync();
    if (user is null)
    {
      throw new UserNotAuthorizedException();
    }
    return user.Id;
  }

  public async Task<List<string>> GetUserRolesAsync(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
      return new List<string>();
    }
    var roles = await _userManager.GetRolesAsync(user);
    return roles.ToList();
  }

  public async Task<bool> IsCurrentUserInRoleAsync(string role)
  {
    var user = await GetCurrentUserAsync();
    var isUserInRole = user is not null && await _userManager.IsInRoleAsync(user, role);
    return isUserInRole;
  }

  public async Task RemoveRoleFromUserAsync(string userId, string roleName)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
      throw new Exception("Usuario no encontrado");
    }
    if (!await _roleManager.RoleExistsAsync(roleName))
    {
      throw new Exception("Rol no encontrado");
    }
    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
    if (!result.Succeeded)
    {
      throw new Exception("Error al eliminar el rol del usuario");
    }
  }

  private async Task<User?> GetCurrentUserAsync()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext is null || httpContext.User is null)
    {
      return null;
    }
    var user = await _userManager.GetUserAsync(httpContext.User);
    return user;
  }
}
