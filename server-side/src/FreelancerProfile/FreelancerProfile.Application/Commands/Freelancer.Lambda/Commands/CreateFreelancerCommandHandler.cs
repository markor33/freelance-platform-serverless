using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerCommands.Lambda.IntegrationEvents;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.Repositories;
using WriteModel;

namespace FreelancerCommands.Lambda.Commands
{
    public class CreateFreelancerCommandHandler
    {
        private readonly IFreelancerRepository _repository;

        public CreateFreelancerCommandHandler()
        {
            _repository = new FreelancerRepository();
        }

        public async Task FunctionHandler(EventBusEvent<FreelancerRegisteredIntegrationEvent> @event, ILambdaContext context)
        {
            try
            {
                var freelancer = Freelancer.Create(@event.Detail.UserId);
                await _repository.SaveAsync(freelancer);

                context.Logger.LogInformation($"Freelancer created: {freelancer}");
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Exception: {ex}");
            }
        }

    }
}
