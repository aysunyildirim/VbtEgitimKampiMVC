using MediatR;
using VbtEgitimKampiMVC.Core.Application.ApiResponse;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

namespace VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;

public class CreateRoleCommandRequest : IRequest<ServiceResponse<CreateRoleCommandResponse>>
{
    public string Name { get; set; } 
    public string Description { get; set; } 
}
