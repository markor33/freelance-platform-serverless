using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ContractCreatedHandler
{
    private readonly IContractReadModelRepository _contractRepository;
    private readonly IJobReadModelRepository _jobRepository;

    public ContractCreatedHandler()
    {
        _contractRepository = new ContractReadModelRepository();
        _jobRepository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ContractCreated> @event, ILambdaContext context)
    {
        var contract = @event.Detail.Contract;
        
        var job = await _jobRepository.GetByIdAsync(@event.Detail.AggregateId);
        job.NumOfActiveContracts += 1;
        var contractViewModel = new ContractViewModel(contract.Id, job.Id, job.ClientId, contract.FreelancerId, contract.Payment, contract.Started, contract.Finished, contract.Status);

        await _contractRepository.SaveAsync(contractViewModel);
        await _jobRepository.SaveAsync(job);
    }

}
