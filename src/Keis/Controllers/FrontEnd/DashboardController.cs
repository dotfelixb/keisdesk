﻿using Keis.Infrastructure.Features.Dashboard.AgentTickets;
using Keis.Model.Commands;
using Keis.Model.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Controllers.FrontEnd;

public class DashboardController : FrontEndController
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IMediator _mediatr;


    public DashboardController(IHttpContextAccessor httpAccessor, ILogger<DashboardController> logger,
        IMediator mediatr) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Index)}");

        return View();
    }

    public async Task<IActionResult> Tickets()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Tickets)}");

        var result = await _mediatr.Send(new AgentTicketsCommand {Id = UserId});
        var statsCard = new StatsCard {Title = "My Tickets", Location = "/dashboard/tickets"};

        if (result.IsSuccess) statsCard.Count = result.Value;
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> Overdue()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Overdue)}");

        var result = await _mediatr.Send(new AgentTicketsCommand {Id = UserId, OverdueOnly = true});
        var statsCard = new StatsCard {Title = "My Overdue Tickets", Location = "/dashboard/tickets"};

        if (result.IsSuccess) statsCard.Count = result.Value;
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> DueToday()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(DueToday)}");

        var result = await _mediatr.Send(new AgentTicketsCommand {Id = UserId, DueToday = true});
        var statsCard = new StatsCard {Title = "My due Today", Location = "/dashboard/tickets"};

        if (result.IsSuccess) statsCard.Count = result.Value;
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> OpenTickets()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(OpenTickets)}");

        var result = await _mediatr.Send(new AgentTicketsCommand {Id = null});
        var statsCard = new StatsCard {Title = "Open Tickets", Location = "/dashboard/tickets"};

        if (result.IsSuccess) statsCard.Count = result.Value;
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> OpenOverdue()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(OpenOverdue)}");

        var result = await _mediatr.Send(new AgentTicketsCommand {Id = null});
        var statsCard = new StatsCard {Title = "Overdue Tickets", Location = "/dashboard/tickets"};

        if (result.IsSuccess) statsCard.Count = result.Value;
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }
}