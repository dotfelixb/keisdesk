using Keis.Infrastructure.Features.BusinessHour.CreateBusinessHour;
using Keis.Infrastructure.Features.BusinessHour.GetBusinessHour;
using Keis.Infrastructure.Features.BusinessHour.ListBusinessHour;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class BusinessHourController : MethodController
{
    private readonly ILogger<BusinessHourController> logger;
    private readonly IMediator mediatr;

    public BusinessHourController(ILogger<BusinessHourController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("businesshour.list", Name = nameof(ListHours))]
    public async Task<IActionResult> ListHours([FromQuery] ListBusinessHourCommand command)
    {
        const string msg = $"GET::{nameof(BusinessHourController)}::{nameof(ListHours)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchBusinessHour>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("businesshour.get", Name = nameof(GetHour))]
    public async Task<IActionResult> GetHour([FromQuery] GetBusinessHourCommand command)
    {
        const string msg = $"GET::{nameof(BusinessHourController)}::{nameof(GetHour)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchBusinessHour>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("businesshour.create", Name = nameof(CreateHour))]
    public async Task<IActionResult> CreateHour(CreateBusinessHourCommand command)
    {
        const string msg = $"POST::{nameof(BusinessHourController)}::{nameof(CreateHour)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/businesshour.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}