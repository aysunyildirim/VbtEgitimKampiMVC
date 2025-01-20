using AutoMapper;
using MediatR;
using VbtEgitimKampiMVC.Core.Application.ApiResponse;
using VbtEgitimKampiMVC.Core.Application.Services.Repositories;

namespace VbtEgitimKampiMVC.Core.Application.Features.Role.Commands.Create;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, ServiceResponse<CreateRoleCommandResponse>>
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<CreateRoleCommandResponse>> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Role>(request);
        var newEntity = await _repository.AddAsync(entity);

        return new ServiceResponse<CreateRoleCommandResponse>
        {
            Data = _mapper.Map<CreateRoleCommandResponse>(newEntity)
        };
    }
}
