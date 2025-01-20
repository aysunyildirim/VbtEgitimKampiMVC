using AutoMapper;
using MediatR;
using VbtEgitimKampiMVC.Core.Application.ApiResponse;
using VbtEgitimKampiMVC.Core.Application.Services.Repositories;
using VbtEgitimKampiMVC.Core.Domain.Entities;

namespace VbtEgitimKampiMVC.Core.Application.Features.User.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, ServiceResponse<CreateUserCommandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IMapper mapper, IUserRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ServiceResponse<CreateUserCommandResponse>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.RegisteredDate = DateTime.Now;
        var newEntity = await _repository.AddAsync(entity);

        return new ServiceResponse<CreateUserCommandResponse>
        {
            Data = _mapper.Map<CreateUserCommandResponse>(newEntity)
        };
    }
}
