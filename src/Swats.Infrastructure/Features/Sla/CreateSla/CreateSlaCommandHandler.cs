using System.Text.Json;
using AutoMapper;
using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Infrastructure.Features.Sla.CreateSla;

public class CreateSlaCommandHandler : IRequestHandler<CreateSlaCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateSlaCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateSlaCommand request, CancellationToken cancellationToken)
    {
        var sla = _mapper.Map<CreateSlaCommand, Model.Domain.Sla>(request);

        var auditLog = new DbAuditLog
        {
            Target = sla.Id,
            ActionName = "sla.create",
            Description = "added sla",
            ObjectName = "sla",
            ObjectData = JsonSerializer.Serialize(sla),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateSla(sla, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(sla.Id) : Result.Fail<string>("Not able to create now!");
    }
}