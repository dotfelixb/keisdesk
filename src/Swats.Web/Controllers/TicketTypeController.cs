using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;
using Swats.Web.Extensions;

namespace Swats.Web.Controllers;

public class TicketTypeController : MethodController
{
    private readonly ILogger<TicketTypeController> logger;
    private readonly IMediator mediatr;

    public TicketTypeController(ILogger<TicketTypeController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("tickettype.list", Name = nameof(ListTypes))]
    public async Task<IActionResult> ListTypes([FromQuery] ListTicketTypeCommand command)
    {
        const string msg = $"GET::{nameof(TicketTypeController)}::{nameof(ListTypes)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }

        return Ok(new ListResult<FetchTicketType>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("tickettype.get", Name = nameof(GetType))]
    public async Task<IActionResult> GetType([FromQuery] GetTicketTypeCommand command)
    {
        const string msg = $"GET::{nameof(TicketTypeController)}::{nameof(GetType)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }

        return Ok(new SingleResult<FetchTicketType>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("tickettype.create", Name = nameof(CreateType))]
    public async Task<IActionResult> CreateType(CreateTicketTypeCommand command)
    {
        const string msg = $"POST::{nameof(TicketTypeController)}::{nameof(CreateType)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }

        var uri = $"/methods/tickettype.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}