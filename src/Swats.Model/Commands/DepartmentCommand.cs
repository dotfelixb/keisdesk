﻿using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateDepartmentCommand : IRequest<Result>
{
    public string Name { get; set; }
    public Guid Manager { get; set; }
    public Guid BusinessHour { get; set; }
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
}

public class CreateTeamCommand : IRequest<Result>
{
    public string Name { get; set; }
    public Guid Lead { get; set; }
    public Guid Department { get; set; }
    public DefaultStatus Status { get; set; }
    public string Notes { get; set; }
}

public class CreateHelpTopicCommand : IRequest<Result>
{
    public string Topic { get; set; }
    public DefaultStatus Status { get; set; }
    public DefaultType Type { get; set; }
    public Guid LinkedDepartment { get; set; }
    public Guid DefaultDepartment { get; set; }
    public string Notes { get; set; }
}

public class CreateTagCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}

public class CreateBusinessHourCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid Timezone{ get; set; }
    public string[] Holidays { get; set; }
    public DefaultStatus Status { get; set; }
}