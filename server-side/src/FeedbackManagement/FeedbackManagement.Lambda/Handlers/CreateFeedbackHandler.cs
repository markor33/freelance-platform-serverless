using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Extensions;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FeedbackManagement.Persistence;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace FeedbackManagement.Lambda.Handlers;

public class CreateFeedbackHandler
{
    private readonly IFeedbackRepository _repository;
    private readonly IValidator<CreateFeedbackCommand> _validator;
    private ILambdaContext _context;

    public CreateFeedbackHandler()
    {
        _repository = new FeedbackRepository();
        _validator = new CreateFeedbackCommandValidator();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = jwtToken.Subject;

        var id = Guid.Parse(request.PathParameters["id"]);

        var command = JsonSerializer.Deserialize<CreateFeedbackCommand>(request.Body, JsonOptions.Options);
        command.ContractId = id;
        command.Role = jwtToken.GetRole();

        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            context.Logger.LogError($"Validation failed - {validationResult.Errors}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Body = $"Validation failed - {validationResult.Errors}"
            };
        }

        var result = await CommandHandler(command);
        var statusCode = result ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode,
            Headers = Headers.CORS
        };
    }

    public async Task<bool> CommandHandler(CreateFeedbackCommand request)
    {
        try
        {
            var finishedContract = await _repository.GetById(request.ContractId);
            var feedback = new Feedback(request.Rating, request.Text);

            if (request.Role == "Employeer")
            {
                finishedContract.SetClientFeedback(feedback);
            }
            else
            {
                finishedContract.SetFreelancerFeedback(feedback);
            }

            await _repository.SaveAsync(finishedContract);

            return true;
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"ERROR: {ex}");

            return false;
        }
    }
}

public class CreateFeedbackCommand
{
    public Guid ContractId { get; set; }
    public string Role { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.Rating).NotEmpty();

        RuleFor(x => x.Text).NotEmpty();
    }
}