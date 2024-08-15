using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using FluentResults;
using JobManagement.Application.IntegrationEvents.Events;
using JobManagement.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class FinishContractCommandHandler : IRequestHandler<FinishContractCommand, Result>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IAmazonEventBridge _eventBridge;

        public FinishContractCommandHandler(IJobRepository jobRepository, IAmazonEventBridge eventBridge)
        {
            _jobRepository = jobRepository;
            _eventBridge = eventBridge;
        }

        public async Task<Result> Handle(FinishContractCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var contract = job.FinishContract(request.ContractId);

            await _jobRepository.SaveAsync(job);
            await PublishEvent(new ContractFinishedIntegrationEvent(contract.Id, job.Id, contract.ClientId, contract.FreelancerId));

            return Result.Ok();
        }

        private async Task PublishEvent(ContractFinishedIntegrationEvent @event)
        {
            var putEventRequest = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    DetailType = "ContractFinishedIntegrationEvent",
                    Detail = JsonSerializer.Serialize(@event),
                    Source = "job-service",
                    EventBusName = "FreelancePlatformEventBus"
                }
            }
            };

            await _eventBridge.PutEventsAsync(putEventRequest);
        }
    }
}
