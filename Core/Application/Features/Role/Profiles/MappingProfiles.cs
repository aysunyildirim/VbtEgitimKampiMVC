using AutoMapper;
using VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

namespace VbtEgitimKampiMVC.Core.Application.Features.Role.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Domain.Entities.Role, CreateRoleCommandRequest>().ReverseMap();
        CreateMap<Domain.Entities.Role, CreateRoleCommandResponse>().ReverseMap();
    }
}
