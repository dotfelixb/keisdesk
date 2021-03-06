using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.GetTeam;

public class GetTeamCommandHandler : IRequestHandler<GetTeamCommand, Result<FetchTeam>>
{
    private readonly IManageRepository _manageRepository;

    public GetTeamCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchTeam>> Handle(GetTeamCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetTeam(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchTeam>($"Team with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}