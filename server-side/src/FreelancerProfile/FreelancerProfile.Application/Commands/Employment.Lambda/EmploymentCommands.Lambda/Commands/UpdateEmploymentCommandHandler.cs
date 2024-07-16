using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentResults;
using FluentValidation;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WriteModel;

namespace EmploymentCommands.Lambda.Commands;

public class UpdateEmploymentCommandHandler
{
    private readonly IFreelancerRepository _freelancerRepository;
    private readonly IValidator<UpdateEmploymentCommand> _validator;
    private ILambdaContext _context;

    public UpdateEmploymentCommandHandler()
    {
        _freelancerRepository = new FreelancerRepository();
        _validator = new UpdateEmploymentCommandValidator();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"]);
        var sub = jwtToken.Subject;

        var id = request.PathParameters["id"];
        if (id != sub)
        {
            context.Logger.LogError("Employment update failed - missing path param");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 401
            };
        }

        var command = JsonSerializer.Deserialize<UpdateEmploymentCommand>(request.Body);
        command.FreelancerId = Guid.Parse(sub);

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
        var statusCode = result.IsSuccess ? 201 : 400;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode
        };
    }

    public async Task<Result> CommandHandler(UpdateEmploymentCommand request)
    {
        try
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(request.FreelancerId);
            if (freelancer is null)
            {
                _context.Logger.LogError($"Freelander with {request.FreelancerId} does not exist");

                return Result.Fail("Employment update failed");
            }

            freelancer.UpdateEmployment(request.EmploymentId, request.Company, request.Title,
                new DateRange(request.Start, request.End), request.Description);

            await _freelancerRepository.SaveAsync(freelancer);

            _context.Logger.LogInformation($"Employment successfully updated - Id:{request.EmploymentId}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"Employment update failed with exception - {ex}");

            return Result.Fail("Edit employment action failed");
        }
    }

}

public class UpdateEmploymentCommand
{
    public Guid FreelancerId { get; set; }
    public Guid EmploymentId { get; set; }
    public string Company { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Description { get; set; }
}

public class UpdateEmploymentCommandValidator : AbstractValidator<UpdateEmploymentCommand>
{
    public UpdateEmploymentCommandValidator()
    {
        RuleFor(x => x.FreelancerId).NotEmpty();

        RuleFor(x => x.EmploymentId).NotEmpty();

        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.Company).NotEmpty();

        RuleFor(x => x.Start).NotEmpty();

        RuleFor(x => x.End).NotEmpty();
    }
}
