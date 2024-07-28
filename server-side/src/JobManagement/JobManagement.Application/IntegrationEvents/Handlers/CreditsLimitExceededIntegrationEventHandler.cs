using JobManagement.Application.Commands.ProposalCommands;
using JobManagement.Application.IntegrationEvents.Events;
using MediatR;

namespace JobManagement.Application.IntegrationEvents.Handlers
{
    public class CreditsLimitExceededIntegrationEventHandler
    {
        private readonly IMediator _mediator;

        public CreditsLimitExceededIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task HandleAsync(CreditsLimitExceededIntegrationEvent @event)
        {
            var command = new DeleteProposalCommand(@event.JobId, @event.ProposalId);
            
            await _mediator.Send(command);
        }

    }
}
