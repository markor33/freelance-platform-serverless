using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace FreelancerCommands.Lambda.Commands;

public class UpdateProfileSummaryCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<UpdateProfileSummaryCommand> _validator;
    private ILambdaContext _context;

    public UpdateProfileSummaryCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new UpdateProfileSummaryCommandValidator();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
     {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var profileSummary = JsonSerializer.Deserialize<ProfileSummary>(request.Body);
        var command = new UpdateProfileSummaryCommand(Guid.Parse(sub), profileSummary);

        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Body = $"Validation failed - {validationResult.Errors}"
            };
        }

        var result = await CommandHandler(command);
        var statusCode = result.IsSuccess ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode
        };
    }

    public async Task<Result> CommandHandler(UpdateProfileSummaryCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
                return Result.Fail("Freelancer does not exist");

            freelancer.UpdateProfileSummary(request.ProfileSummary);

            await _freelancerRepository.SaveAsync(freelancer);

            return Result.Ok();
        }
        catch
        {
            return Result.Fail("Freelancer profile setup failed");
        }
    }
}

public class UpdateProfileSummaryCommand
{
    public Guid FreelancerId { get; private set; }
    public ProfileSummary ProfileSummary { get; private set; }

    public UpdateProfileSummaryCommand(Guid freelancerId, ProfileSummary profileSummary)
    {
        FreelancerId = freelancerId;
        ProfileSummary = profileSummary;
    }
}

public class UpdateProfileSummaryCommandValidator : AbstractValidator<UpdateProfileSummaryCommand>
{
    public UpdateProfileSummaryCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.ProfileSummary).NotEmpty();
    }
}