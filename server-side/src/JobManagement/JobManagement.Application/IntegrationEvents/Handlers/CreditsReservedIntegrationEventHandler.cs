using JobManagement.Application.Commands.ProposalCommands;
using JobManagement.Application.IntegrationEvents.Events;
using MediatR;

namespace JobManagement.Application.IntegrationEvents.Handlers
{
    public class CreditsReservedIntegrationEventHandler
    {
        private readonly IMediator _mediator;

        public CreditsReservedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task HandleAsync(CreditsReservedIntegrationEvent @event)
        {
            var command = new ProcessReservedCreditsCommand(@event.JobId, @event.ProposalId);
            await _mediator.Send(command);
        }

    }
}
