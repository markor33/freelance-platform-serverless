using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Common.Layer.Extensions;
using FreelancerCommands.Lambda.IntegrationEvents;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.Repositories;
using WriteModel;

namespace FreelancerCommands.Lambda.Handlers
{
    public class CreateFreelancer
    {
        private readonly IFreelancerRepository _repository;

        public CreateFreelancer()
        {
            _repository = new FreelancerRepository();
        }

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            var freelancerRegisteredEvents = sqsEvent.Records
                .Select(sqsMessage => sqsMessage.DeserializeSNSMessage<FreelancerRegisteredIntegrationEvent>());
            var tasks = freelancerRegisteredEvents.Select(@event => IntegrationEventHandler(@event));

            await Task.WhenAll(tasks);
        }

        private async Task IntegrationEventHandler(FreelancerRegisteredIntegrationEvent @event)
        {
            var freelancer = Freelancer.Create(@event.UserId);
            await _repository.SaveAsync(freelancer);
        }
    }
}
