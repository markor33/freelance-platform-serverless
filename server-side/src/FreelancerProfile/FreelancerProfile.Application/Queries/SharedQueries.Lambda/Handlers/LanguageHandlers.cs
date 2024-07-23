using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FreelancerProfile.Domain.Repositories;
using ReadModelStore;
using System.Text.Json;

namespace SharedQueries.Lambda.Handlers;

public class LanguageHandlers
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageHandlers()
    {
        _languageRepository = new LanguageRepository();
    }

    public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var languages = await _languageRepository.Get();

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(languages, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
