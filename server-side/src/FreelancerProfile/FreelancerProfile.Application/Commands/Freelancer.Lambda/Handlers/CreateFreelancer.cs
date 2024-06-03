using Amazon.Lambda.Core;
using Common.Layer.EventBus;
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

        public async Task FunctionHandler(EventBusEvent<FreelancerRegisteredIntegrationEvent> @event, ILambdaContext context)
        {
            var freelancer = Freelancer.Create(@event.Detail.UserId);
            await _repository.SaveAsync(freelancer);
        }

    }
}
