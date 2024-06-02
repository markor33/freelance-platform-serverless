using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateFreelancer.Lambda;

public class Function
{

    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {

    }
}
