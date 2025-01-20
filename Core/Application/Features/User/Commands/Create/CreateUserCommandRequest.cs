using MediatR;
using VbtEgitimKampiMVC.Core.Application.ApiResponse;

namespace VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

public class CreateUserCommandRequest : IRequest<ServiceResponse<CreateUserCommandResponse>>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}
