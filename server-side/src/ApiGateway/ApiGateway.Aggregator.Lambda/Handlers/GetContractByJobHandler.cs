using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ApiGateway.Aggregator.Lambda.Models;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;
using System.Text.Json;

namespace ApiGateway.Aggregator.Lambda.Handlers;

public class GetContractByJobHandler
{
    private readonly IContractReadModelRepository _contractRepository;
    private readonly IFreelancerReadModelRepository _freelancerRepository;
    private readonly IJobReadModelRepository _jobRepository;

    private Dictionary<Guid, JobViewModel> _jobs;
    private Dictionary<Guid, FreelancerViewModel> _freelancers;

    public GetContractByJobHandler()
    {
        _contractRepository = new ContractReadModelRepository();
        _freelancerRepository = new FreelancerReadModelRepository();
        _jobRepository = new JobReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jobId = Guid.Parse(request.PathParameters["jobId"]);
        try
        {
            var contracts = await _contractRepository.GetByJob(jobId);
            _freelancers = (await _freelancerRepository.GetByIdsAsync(contracts.Select(x => x.FreelancerId).ToHashSet())).ToDictionary(x => x.Id);

            var contractsAggregated = new List<Contract>();
            foreach (var contract in contracts)
            {
                _freelancers.TryGetValue(contract.FreelancerId, out FreelancerViewModel freelancer);
                contractsAggregated.Add(new Contract(contract, freelancer));
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(contractsAggregated, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex.ToString());

            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS
            };
        }
    }

}
