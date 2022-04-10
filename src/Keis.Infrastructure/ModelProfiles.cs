﻿using AutoMapper;
using Keis.Model.Commands;
using Keis.Model.Domain;

namespace Keis.Infrastructure;

public class ModelProfiles : Profile
{
    public ModelProfiles()
    {
        CreateMap<CreateTicketCommand, Ticket>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTicketTypeCommand, TicketType>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateBusinessHourCommand, BusinessHour>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateAgentCommand, Agent>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateAgentCommand, AuthUser>()
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Mobile))
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateDepartmentCommand, Department>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTeamCommand, Team>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateHelpTopicCommand, HelpTopic>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTagCommand, Tag>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateSlaCommand, Sla>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<LoginLogCommand, LoginAudit>();
        
        /**
         *.ForMember(d => d.ParentCustomer,
         *          opt => opt.MapFrom(s => s.ParentCustomer.ToGuid()))
         */
    }
}