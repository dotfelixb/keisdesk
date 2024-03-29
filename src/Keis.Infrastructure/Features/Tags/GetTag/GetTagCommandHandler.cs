﻿using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tags.GetTag;

public class GetTagCommandHandler : IRequestHandler<GetTagCommand, Result<FetchTag>>
{
    private readonly IManageRepository _manageRepository;

    public GetTagCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchTag>> Handle(GetTagCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetTag(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchTag>($"Tag with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}