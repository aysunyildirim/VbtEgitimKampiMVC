using AutoMapper;
using VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

namespace VbtEgitimKampiMVC.Core.Application.Features.User.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Domain.Entities.User, CreateUserCommandRequest>().ReverseMap();
        CreateMap<Domain.Entities.User, CreateUserCommandResponse>().ReverseMap();
    }
}
