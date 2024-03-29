﻿using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Department.GetDepartment;

public class GetDepartmentCommandHandler : IRequestHandler<GetDepartmentCommand, Result<FetchDepartment>>
{
    private readonly IManageRepository _manageRepository;

    public GetDepartmentCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchDepartment>> Handle(GetDepartmentCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetDepartment(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchDepartment>($"Department with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}