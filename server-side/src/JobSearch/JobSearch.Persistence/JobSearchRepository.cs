using Amazon.Runtime;
using Aws4RequestSigner;
using JobSearch.IndexModel;
using System.Text;
using System.Text.Json;

namespace JobSearch.Persistence;

public class JobSearchRepository : IJobSearchRepository
{
    private readonly ImmutableCredentials _credentials;
    private readonly string _accessKey = Environment.GetEnvironmentVariable("ACCESS_KEY");
    private readonly string _secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
    private readonly string _openSearchUrl = Environment.GetEnvironmentVariable("OPENSEARCH_URL");
    private readonly string _indexName = "jobs";
    private readonly string _region = "eu-central-1";
    private readonly string _service = "es";
    private readonly AWS4RequestSigner _signer;
    private readonly HttpClient _httpClient;

    public JobSearchRepository()
    {
        ArgumentNullException.ThrowIfNull(_openSearchUrl);
        ArgumentNullException.ThrowIfNull(_accessKey);
        ArgumentNullException.ThrowIfNull(_secretKey);

        _signer = new AWS4RequestSigner(_accessKey, _secretKey);
        _httpClient = new HttpClient();
    }

    public async Task<Job> GetById(Guid id)
    {
        var response = await SendRequest(HttpMethod.Get, $"{_indexName}/_doc/{id}");
        var searchResponse = JsonSerializer.Deserialize<GetDocResponse>(await response.Content.ReadAsStringAsync());

        if (searchResponse?._source == null)
        {
            throw new Exception($"Job with ID {id} not found.");
        }

        return searchResponse._source;
    }

    public async Task<List<Job>> Search(JobSearchParams jobSearchParams)
    {
        var response = await SendRequest(HttpMethod.Post, $"{_indexName}/_search", jobSearchParams.BuildSearchQuery());
        var str = await response.Content.ReadAsStringAsync();
        var searchResponse = JsonSerializer.Deserialize<PostSearchResponse>(await response.Content.ReadAsStringAsync());

        return searchResponse.hits.hits.Select(x => x._source).ToList();
    }

    public async Task Index(Job job)
    {
        await SendRequest(HttpMethod.Put, $"{_indexName}/_doc/{job.Id}", JsonSerializer.Serialize(job));
    }

    public async Task Update(Job job)
    {
        var updatePayload = new
        {
            doc = job
        };

        await SendRequest(HttpMethod.Post, $"{_indexName}/_update/{job.Id}", JsonSerializer.Serialize(updatePayload));
    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string path, string? jsonData = null)
    {
        var requestUri = new Uri($"{_openSearchUrl}/{path}");

        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = requestUri
        };

        if (jsonData != null)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            request.Content = content;
        }

        request = await _signer.Sign(request, _service, _region);
        var response =  await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to index job: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }

        return response;
    }
}