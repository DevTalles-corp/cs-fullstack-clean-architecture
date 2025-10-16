using System;
using MediatR;
using TechNotes.Domain.User;

namespace TechNotes.Application.Users.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserResponse>>>
{
  private readonly IUserRepository _userRepository;
  private readonly IUserService _userService;
  public GetUsersQueryHandler(IUserRepository userRepository, IUserService userService)
  {
    _userRepository = userRepository;
    _userService = userService;
  }
  public async Task<Result<List<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
  {
    if (!await _userService.IsCurrentUserInRoleAsync("Admin"))
    {
      return Result.Fail<List<UserResponse>>("No esta autorizado para ver todos los usuarios");
    }
    var users = await _userRepository.GetAllUsersAsync();
    var response = users.Adapt<List<UserResponse>>();
    return response;
  }
}
