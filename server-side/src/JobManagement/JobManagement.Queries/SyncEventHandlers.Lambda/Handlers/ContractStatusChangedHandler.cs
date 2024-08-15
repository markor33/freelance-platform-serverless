using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ContractStatusChangedHandler
{
    private readonly IContractReadModelRepository _contractRepository;
    private readonly IJobReadModelRepository _jobRepository;

    public ContractStatusChangedHandler()
    {
        _contractRepository = new ContractReadModelRepository();
        _jobRepository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ContractStatusChanged> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var contract  = await _contractRepository.GetById(detail.ContractId, detail.AggregateId);
        contract.Status = detail.Status;

        var job = await _jobRepository.GetByIdAsync(detail.AggregateId);
        if (contract.Status == ContractStatus.FINISHED)
        {
            job.NumOfActiveContracts -= 1;
            job.NumOfFinishedContracts += 1;
            contract.Finished = DateTime.Now;
        }
        else if (contract.Status == ContractStatus.TERMINATED)
        {
            job.NumOfActiveContracts -= 1;
        }

        await _contractRepository.SaveAsync(contract);
        await _jobRepository.SaveAsync(job);
    }
}
