using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.Tickets.CreateComment;

public class CreateTicketCommentCommandValidator : AbstractValidator<CreateTicketCommentCommand>
{
    public CreateTicketCommentCommandValidator()
    {
        RuleFor(r => r.Body).NotNull();
    }
}