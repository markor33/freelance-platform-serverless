using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FreelancerProfile.Domain.Repositories;
using ReadModelStore;
using System.Text.Json;

namespace SharedQueries.Lambda.Handlers;

public class ProfessionHandlers
{
    private readonly IProfessionRepository _professionRepository;

    public ProfessionHandlers()
    {
        _professionRepository = new ProfessionRepository();
    }

    public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var professions = await _professionRepository.Get();

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(professions, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
