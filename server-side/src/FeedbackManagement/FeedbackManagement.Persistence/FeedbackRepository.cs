using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections;

namespace FeedbackManagement.Persistence;

public interface IFeedbackRepository
{
    Task<FinishedContract> GetById(Guid id);
    Task<List<FinishedContract>> GetByFreelancer(Guid freelancerId);
    Task<Dictionary<Guid, double>> GetAverageRatingByFreelancers(HashSet<Guid> freelancerIds);
    Task<Dictionary<Guid, double>> GetAverageRatingByClients(HashSet<Guid> clientIds);
    Task SaveAsync(FinishedContract finishedContract);
}

public class FeedbackRepository : IFeedbackRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public FeedbackRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<FinishedContract> GetById(Guid id)
    {
        return await _context.LoadAsync<FinishedContract>(id);
    }

    public async Task<List<FinishedContract>> GetByFreelancer(Guid freelancerId)
    {
        var queryConditions = new List<ScanCondition>
        {
            new("FreelancerId", ScanOperator.Equal, freelancerId),
            new("ClientFeedback", ScanOperator.IsNotNull)
        };

        var search = _context.ScanAsync<FinishedContract>(queryConditions);

        return await search.GetNextSetAsync();
    }

    public async Task<Dictionary<Guid, double>> GetAverageRatingByFreelancers(HashSet<Guid> freelancerIds)
    {
        var result = new Dictionary<Guid, double>();

        var queryConditions = new List<ScanCondition>
        {
            new("FreelancerId", ScanOperator.In, freelancerIds.Select(x => (object)x).ToArray()),
            new("ClientFeedback", ScanOperator.IsNotNull)
        };

        var search = _context.ScanAsync<FinishedContract>(queryConditions);

        var allContracts = new List<FinishedContract>();
        do
        {
            var batch = await search.GetNextSetAsync();
            allContracts.AddRange(batch);
        } while (!search.IsDone);

        var groupedContracts = allContracts.GroupBy(fc => fc.FreelancerId);

        foreach (var group in groupedContracts)
        {
            var id = group.Key;
            var feedbacks = group.Select(fc => fc.ClientFeedback).Where(f => f != null);

            if (feedbacks.Any())
            {
                var averageRating = feedbacks.Average(f => f.Rating);
                result[id] = averageRating;
            }
            else
            {
                result[id] = 0;
            }
        }

        foreach (var id in freelancerIds)
        {
            if (!result.ContainsKey(id))
            {
                result[id] = 0;
            }
        }

        return result;
    }

    public async Task<Dictionary<Guid, double>> GetAverageRatingByClients(HashSet<Guid> clientIds)
    {
        var result = new Dictionary<Guid, double>();

        var queryConditions = new List<ScanCondition>
        {
            new("ClientId", ScanOperator.In, clientIds.Select(x => (object)x).ToArray()),
            new("FreelancerFeedback", ScanOperator.IsNotNull)
        };

        var search = _context.ScanAsync<FinishedContract>(queryConditions);

        var allContracts = new List<FinishedContract>();
        do
        {
            var batch = await search.GetNextSetAsync();
            allContracts.AddRange(batch);
        } while (!search.IsDone);

        var groupedContracts = allContracts.GroupBy(fc => fc.ClientId);

        foreach (var group in groupedContracts)
        {
            var id = group.Key;
            var feedbacks = group.Select(fc => fc.FreelancerFeedback).Where(f => f != null);

            if (feedbacks.Any())
            {
                var averageRating = feedbacks.Average(f => f.Rating);
                result[id] = averageRating;
            }
            else
            {
                result[id] = 0;
            }
        }

        foreach (var id in clientIds)
        {
            if (!result.ContainsKey(id))
            {
                result[id] = 0;
            }
        }

        return result;
    }

    public async Task SaveAsync(FinishedContract finishedContract)
    {
        await _context.SaveAsync<FinishedContract>(finishedContract);
    }
}
