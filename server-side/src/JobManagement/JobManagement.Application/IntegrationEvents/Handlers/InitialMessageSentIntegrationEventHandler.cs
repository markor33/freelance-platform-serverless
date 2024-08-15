using JobManagement.Application.Commands.ProposalCommands;
using JobManagement.Application.IntegrationEvents.Events;
using MediatR;

namespace JobManagement.Application.IntegrationEvents.Handlers
{
    public class InitialMessageSentIntegrationEventHandler
    {
        private readonly IMediator _mediator;

        public InitialMessageSentIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task HandleAsync(InitialMessageSentIntegrationEvent @event)
        {
            var command = new ProcessInitialMessageSentCommand(@event.JobId, @event.ProposalId, @event.FreelancerId);
            await _mediator.Send(command);
        }
    }

}
